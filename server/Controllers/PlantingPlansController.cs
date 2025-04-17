using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.AppDbContext;
using server.Models;

namespace server.Controllers
{
    /// <summary>
    /// Контроллер для управления планами посева
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PlantingPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlantingPlansController> _logger;

        public PlantingPlansController(
            ApplicationDbContext context,
            ILogger<PlantingPlansController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        // /// <summary>
        // /// Инициализирует таблицу PlantingPlans тестовыми данными
        // /// </summary>
        // /// <returns>Результат операции</returns>
        // /// <response code="200">Данные успешно добавлены</response>
        // /// <response code="500">Ошибка при добавлении данных</response>
        // [HttpPost("initialize")]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(500)]
        // public async Task<ActionResult<string>> InitializePlantingPlansData()
        // {
        //     try
        //     {
        //         // Проверяем, есть ли уже данные
        //         if (await _context.PlantingPlans.AnyAsync())
        //         {
        //             return Ok("Таблица PlantingPlans уже содержит данные");
        //         }

        //         // Получаем необходимые данные для создания планов посадки
        //         var fields = await _context.Fields.ToListAsync();
        //         var crops = await _context.Crops.ToListAsync();
        //         var seasons = await _context.Seasons.ToListAsync();

        //         if (!fields.Any() || !crops.Any() || !seasons.Any())
        //         {
        //             return BadRequest(
        //                 "Необходимые данные для создания планов посадки отсутствуют. Убедитесь, что таблицы Fields, Crops и Seasons содержат данные."
        //             );
        //         }

        //         // Добавляем планы посадки
        //         var plantingPlans = new PlantingPlans[]
        //         {
        //             new PlantingPlans
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Field_id = fields[0].Id,
        //                 Fields = fields[0],
        //                 Crop_id = crops[0].Id,
        //                 Crops = crops[0],
        //                 Season_id = seasons[0].Id,
        //                 Planned_date = DateTime.UtcNow.AddDays(30),
        //                 Expected_yield = 4000,
        //             },
        //             new PlantingPlans
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Field_id = fields[1].Id,
        //                 Fields = fields[1],
        //                 Crop_id = crops[1].Id,
        //                 Crops = crops[1],
        //                 Season_id = seasons[1].Id,
        //                 Planned_date = DateTime.UtcNow.AddDays(45),
        //                 Expected_yield = 6000,
        //             },
        //         };

        //         await _context.PlantingPlans.AddRangeAsync(plantingPlans);
        //         await _context.SaveChangesAsync();

        //         return Ok("Данные успешно добавлены в таблицу PlantingPlans");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Ошибка при инициализации данных в таблице PlantingPlans");
        //         return StatusCode(500, "Ошибка при инициализации данных в таблице PlantingPlans");
        //     }
        // }

        /// <summary>
        /// Автоматическое составление плана посева
        /// </summary>
        /// <param name="request">Данные для генерации плана</param>
        /// <returns>Сгенерированный план посева</returns>
        /// <response code="200">Возвращает сгенерированный план посева</response>
        /// <response code="400">Неверные входные данные</response>
        /// <response code="404">Культура, поле или сезон не найдены</response>
        [HttpPost("generate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PlantingPlanResponse>> GeneratePlantingPlan(
            PlantingPlanRequest request
        )
        {
            try
            {
                var crop = await _context.Crops.FindAsync(request.CropId);
                if (crop == null)
                    return NotFound("Культура не найдена");

                var field = await _context.Fields.FindAsync(request.FieldId);
                if (field == null)
                    return NotFound("Поле не найдено");

                var season = await _context.Seasons.FindAsync(request.SeasonId);
                if (season == null)
                    return NotFound("Сезон не найден");

                var recommendedDate = CalculateOptimalPlantingDate(crop, season, field);

                var plantingPlan = new PlantingPlans
                {
                    Crop_id = request.CropId,
                    Field_id = request.FieldId,
                    Season_id = request.SeasonId,
                    Planned_date = recommendedDate,
                    Expected_yield = 0,
                };

                _context.PlantingPlans.Add(plantingPlan);
                await _context.SaveChangesAsync();

                var response = new PlantingPlanResponse
                {
                    PlanId = plantingPlan.Id,
                    RecommendedDate = recommendedDate.ToString("yyyy-MM-dd"),
                    Notes = $"Лучший период для посева {crop.Title} на поле {field.Title}.",
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при генерации плана посева");
                return BadRequest("Произошла ошибка при генерации плана посева");
            }
        }

        private DateTime CalculateOptimalPlantingDate(Crops crop, Seasons season, Fields field)
        {
            var baseDate = season.Start_date;

            var daysToAdd = 0;

            if (crop.Title.Contains("Пшеница"))
                daysToAdd = 5;
            else if (crop.Title.Contains("Кукуруза"))
                daysToAdd = 10;
            else if (crop.Title.Contains("Подсолнечник"))
                daysToAdd = 15;

            return baseDate.AddDays(daysToAdd);
        }

        private string GeneratePlantingNotes(Crops crop, Fields field)
        {
            return $"Лучший период для посева {crop.Title} на поле {field.Title}.";
        }
    }

    public class PlantingPlanRequest
    {
        public Guid CropId { get; set; }

        public Guid FieldId { get; set; }

        public Guid SeasonId { get; set; }
    }

    public class PlantingPlanResponse
    {
        public Guid PlanId { get; set; }

        public string RecommendedDate { get; set; }

        public string Notes { get; set; }
    }
}
