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
    /// Контроллер для управления полями
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FieldsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FieldsController> _logger;

        public FieldsController(ApplicationDbContext context, ILogger<FieldsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Возвращает геоданные полей и их статус для визуализации на карте
        /// </summary>
        /// <returns>Список полей с геоданными и статусом</returns>
        /// <response code="200">Возвращает геоданные полей</response>
        /// <response code="500">Ошибка при получении данных</response>
        [HttpGet("map-status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<FieldMapStatus>>> GetFieldsMapStatus()
        {
            try
            {
                var fields = await _context
                    .Fields.Include(f => f.PlantingPlans)
                    .ThenInclude(p => p.Crops)
                    .Include(f => f.PlantingPlans)
                    .ThenInclude(p => p.HarvestLogs)
                    .ToListAsync();

                var fieldMapStatuses = new List<FieldMapStatus>();

                foreach (var field in fields)
                {
                    var currentPlan = field
                        .PlantingPlans.Where(p =>
                            p.Planned_date <= DateTime.Now && p.HarvestLogs == null
                        )
                        .OrderByDescending(p => p.Planned_date)
                        .FirstOrDefault();

                    var lastHarvest = field
                        .PlantingPlans.Where(p => p.HarvestLogs != null)
                        .Select(p => p.HarvestLogs)
                        .OrderByDescending(h => h.Harvest_date)
                        .FirstOrDefault();

                    var status = DetermineFieldStatus(field, currentPlan, lastHarvest);

                    var fieldMapStatus = new FieldMapStatus
                    {
                        FieldId = field.Id,
                        FieldName = field.Title,
                        Area = field.Area,
                        SoilType = field.Soil_type,
                        Coordinates = field.Coordinates,
                        Status = status,
                        CurrentCrop = currentPlan?.Crops?.Title,
                        PlantingDate = currentPlan?.Planned_date,
                        ExpectedHarvestDate = currentPlan?.Planned_date.AddDays(
                            currentPlan?.Crops?.Growth_period ?? 0
                        ),
                        LastHarvestDate = lastHarvest?.Harvest_date,
                        LastHarvestYield = lastHarvest?.Actual_yield,
                        Color = GetStatusColor(status),
                    };

                    fieldMapStatuses.Add(fieldMapStatus);
                }

                return Ok(fieldMapStatuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении геоданных полей");
                return StatusCode(500, "Ошибка при получении геоданных полей");
            }
        }

        private string DetermineFieldStatus(
            Fields field,
            PlantingPlans currentPlan,
            HarvestLogs lastHarvest
        )
        {
            if (currentPlan == null)
            {
                return "Свободно";
            }

            var daysSincePlanting = (DateTime.Now - currentPlan.Planned_date).TotalDays;
            var growthPeriod = currentPlan.Crops?.Growth_period ?? 0;

            if (daysSincePlanting < 0)
            {
                return "Запланирована посадка";
            }
            else if (daysSincePlanting < growthPeriod * 0.25)
            {
                return "Ранний рост";
            }
            else if (daysSincePlanting < growthPeriod * 0.5)
            {
                return "Активный рост";
            }
            else if (daysSincePlanting < growthPeriod * 0.75)
            {
                return "Цветение";
            }
            else if (daysSincePlanting < growthPeriod)
            {
                return "Созревание";
            }
            else
            {
                return "Готово к уборке";
            }
        }

        private string GetStatusColor(string status)
        {
            return status switch
            {
                "Свободно" => "#CCCCCC", // Серый
                "Запланирована посадка" => "#FFD700", // Золотой
                "Ранний рост" => "#90EE90", // Светло-зеленый
                "Активный рост" => "#32CD32", // Зеленый
                "Цветение" => "#FF69B4", // Розовый
                "Созревание" => "#FFA500", // Оранжевый
                "Готово к уборке" => "#FF4500", // Красновато-оранжевый
                _ => "#CCCCCC", // По умолчанию серый
            };
        }
    }

    public class FieldMapStatus
    {
        public Guid FieldId { get; set; }

        public string FieldName { get; set; }

        public double Area { get; set; }

        public string SoilType { get; set; }

        public string Coordinates { get; set; }

        public string Status { get; set; }

        public string CurrentCrop { get; set; }

        public DateTime? PlantingDate { get; set; }

        public DateTime? ExpectedHarvestDate { get; set; }

        public DateTime? LastHarvestDate { get; set; }

        public double? LastHarvestYield { get; set; }

        public string Color { get; set; }
    }
}
