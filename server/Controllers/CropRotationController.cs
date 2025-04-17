using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.AppDbContext;
using server.Models;

namespace server.Controllers
{
    /// <summary>
    /// Контроллер для контроля севооборота
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CropRotationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CropRotationController> _logger;

        public CropRotationController(
            ApplicationDbContext context,
            ILogger<CropRotationController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Проверяет нарушения севооборота на указанном поле
        /// </summary>
        /// <param name="field_id">ID поля</param>
        /// <returns>Список предупреждений о нарушениях севооборота</returns>
        /// <response code="200">Возвращает список предупреждений</response>
        /// <response code="404">Поле не найдено</response>
        [HttpGet("warnings")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CropRotationWarningsResponse>> GetRotationWarnings(
            Guid field_id
        )
        {
            var field = await _context.Fields.FindAsync(field_id);
            if (field == null)
            {
                return NotFound($"Поле с ID {field_id} не найдено");
            }

            var plantingHistory = await _context
                .PlantingPlans.Include(p => p.Crops)
                .Where(p => p.Field_id == field_id)
                .OrderByDescending(p => p.Planned_date)
                .Take(10)
                .ToListAsync();

            var warnings = new List<RotationWarning>();

            if (plantingHistory.Count() >= 2)
            {
                for (int i = 0; i < plantingHistory.Count() - 1; i++)
                {
                    var currentPlan = plantingHistory[i];
                    var previousPlan = plantingHistory[i + 1];

                    if (currentPlan.Crop_id == previousPlan.Crop_id)
                    {
                        warnings.Add(
                            new RotationWarning
                            {
                                Type = "repeated_crop",
                                Severity = "high",
                                Message =
                                    $"Культура '{currentPlan.Crops?.Title}' высаживается на этом поле два года подряд. "
                                    + $"Рекомендуется соблюдать севооборот для сохранения плодородия почвы.",
                                Year = currentPlan.Planned_date.Year,
                            }
                        );
                    }

                    var yearsBetweenPlantings =
                        (currentPlan.Planned_date - previousPlan.Planned_date).TotalDays / 365;
                    if (yearsBetweenPlantings < 2)
                    {
                        warnings.Add(
                            new RotationWarning
                            {
                                Type = "short_interval",
                                Severity = "medium",
                                Message =
                                    $"Слишком короткий интервал между посадками культур. "
                                    + $"Рекомендуемый интервал - минимум 2 года.",
                                Year = currentPlan.Planned_date.Year,
                            }
                        );
                    }
                }

                var uniqueCrops = plantingHistory.Select(p => p.Crop_id).Distinct().Count();
                if (uniqueCrops < 3 && plantingHistory.Count() >= 5)
                {
                    warnings.Add(
                        new RotationWarning
                        {
                            Type = "low_diversity",
                            Severity = "medium",
                            Message =
                                "Низкое разнообразие культур в севообороте. "
                                + "Рекомендуется включить больше различных культур для улучшения плодородия почвы.",
                            Year = DateTime.Now.Year,
                        }
                    );
                }
            }

            var response = new CropRotationWarningsResponse
            {
                FieldId = field_id,
                FieldName = field.Title,
                Warnings = warnings,
                HasWarnings = warnings.Any(),
            };

            return Ok(response);
        }
    }

    public class RotationWarning
    {
        public string Type { get; set; }

        public string Severity { get; set; }

        public string Message { get; set; }

        public int Year { get; set; }
    }

    public class CropRotationWarningsResponse
    {
        public Guid FieldId { get; set; }

        public string FieldName { get; set; }

        public List<RotationWarning> Warnings { get; set; }

        public bool HasWarnings { get; set; }
    }
}
