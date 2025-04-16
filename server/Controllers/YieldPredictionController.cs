using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.AppDbContext;
using server.Models;

namespace server.Controllers
{
    /// <summary>
    /// Контроллер для прогнозирования урожайности
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class YieldPredictionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<YieldPredictionController> _logger;

        public YieldPredictionController(
            ApplicationDbContext context,
            ILogger<YieldPredictionController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Получает прогноз урожайности для указанной культуры и поля
        /// </summary>
        /// <param name="crop_id">ID культуры</param>
        /// <param name="field_id">ID поля</param>
        /// <returns>Прогноз урожайности с уровнем уверенности</returns>
        /// <response code="200">Возвращает прогноз урожайности</response>
        /// <response code="400">Неверные входные данные</response>
        /// <response code="404">Культура или поле не найдены</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<YieldPredictionResponse>> GetYieldPrediction(
            Guid crop_id,
            Guid field_id
        )
        {
            // Проверяем существование культуры и поля
            var crop = await _context.Crops.FindAsync(crop_id);
            if (crop == null)
            {
                return NotFound($"Культура с ID {crop_id} не найдена");
            }

            var field = await _context.Fields.FindAsync(field_id);
            if (field == null)
            {
                return NotFound($"Поле с ID {field_id} не найдено");
            }

            // Получаем историю урожаев для данной культуры на данном поле
            var harvestHistory = await _context
                .PlantingPlans.Include(p => p.HarvestLogs)
                .Where(p => p.Crop_id == crop_id && p.Field_id == field_id && p.HarvestLogs != null)
                .OrderByDescending(p => p.Planned_date)
                .Take(5)
                .Select(p => p.HarvestLogs)
                .ToListAsync();

            // Простой алгоритм прогнозирования на основе исторических данных
            double predictedYield = 0;
            double confidence = 0.7; // Базовый уровень уверенности

            if (harvestHistory.Any())
            {
                // Средняя урожайность за последние 5 лет
                predictedYield = harvestHistory.Average(h => h.Actual_yield);

                // Корректируем прогноз на основе качества почвы
                if (!string.IsNullOrEmpty(field.Soil_type))
                {
                    // Если тип почвы оптимальный для культуры
                    if (field.Soil_type.ToLower().Contains("чернозем"))
                    {
                        predictedYield *= 1.1;
                        confidence += 0.05;
                    }
                }

                // Корректируем прогноз на основе оптимальной температуры
                if (Math.Abs(crop.Optimal_temperature - 20) <= 5) // Предполагаем, что 20°C - оптимальная температура
                {
                    predictedYield *= 1.05;
                    confidence += 0.05;
                }
            }
            else
            {
                // Если нет исторических данных, используем ожидаемую урожайность из последнего плана посадки
                var lastPlan = await _context
                    .PlantingPlans.Where(p => p.Crop_id == crop_id && p.Field_id == field_id)
                    .OrderByDescending(p => p.Planned_date)
                    .FirstOrDefaultAsync();

                if (lastPlan != null)
                {
                    predictedYield = lastPlan.Expected_yield;
                }
                else
                {
                    // Если нет планов посадки, используем базовую урожайность
                    predictedYield = 3.0; // Базовое значение
                }
                confidence = 0.5; // Меньшая уверенность при отсутствии исторических данных
            }

            // Ограничиваем уверенность максимальным значением 0.95
            confidence = Math.Min(confidence, 0.95);

            var response = new YieldPredictionResponse
            {
                PredictedYield = Math.Round(predictedYield, 2),
                Unit = "тонн/га",
                Confidence = Math.Round(confidence, 2),
            };

            return Ok(response);
        }
    }

    /// <summary>
    /// Модель ответа с прогнозом урожайности
    /// </summary>
    public class YieldPredictionResponse
    {
        /// <summary>
        /// Прогнозируемая урожайность
        /// </summary>
        public double PredictedYield { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Уровень уверенности в прогнозе (от 0 до 1)
        /// </summary>
        public double Confidence { get; set; }
    }
}
