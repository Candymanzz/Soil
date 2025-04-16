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

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="logger">Логгер</param>
        public PlantingPlansController(
            ApplicationDbContext context,
            ILogger<PlantingPlansController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Инициализирует таблицу PlantingPlans тестовыми данными
        /// </summary>
        /// <returns>Результат операции</returns>
        /// <response code="200">Данные успешно добавлены</response>
        /// <response code="500">Ошибка при добавлении данных</response>
        [HttpPost("initialize")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> InitializePlantingPlansData()
        {
            try
            {
                // Проверяем, есть ли уже данные
                if (await _context.PlantingPlans.AnyAsync())
                {
                    return Ok("Таблица PlantingPlans уже содержит данные");
                }

                // Получаем необходимые данные для создания планов посадки
                var fields = await _context.Fields.ToListAsync();
                var crops = await _context.Crops.ToListAsync();
                var seasons = await _context.Seasons.ToListAsync();

                if (!fields.Any() || !crops.Any() || !seasons.Any())
                {
                    return BadRequest(
                        "Необходимые данные для создания планов посадки отсутствуют. Убедитесь, что таблицы Fields, Crops и Seasons содержат данные."
                    );
                }

                // Добавляем планы посадки
                var plantingPlans = new PlantingPlans[]
                {
                    new PlantingPlans
                    {
                        Id = Guid.NewGuid(),
                        Field_id = fields[0].Id,
                        Fields = fields[0],
                        Crop_id = crops[0].Id,
                        Crops = crops[0],
                        Season_id = seasons[0].Id,
                        Planned_date = DateTime.UtcNow.AddDays(30),
                        Expected_yield = 4000,
                    },
                    new PlantingPlans
                    {
                        Id = Guid.NewGuid(),
                        Field_id = fields[1].Id,
                        Fields = fields[1],
                        Crop_id = crops[1].Id,
                        Crops = crops[1],
                        Season_id = seasons[1].Id,
                        Planned_date = DateTime.UtcNow.AddDays(45),
                        Expected_yield = 6000,
                    },
                };

                await _context.PlantingPlans.AddRangeAsync(plantingPlans);
                await _context.SaveChangesAsync();

                return Ok("Данные успешно добавлены в таблицу PlantingPlans");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при инициализации данных в таблице PlantingPlans");
                return StatusCode(500, "Ошибка при инициализации данных в таблице PlantingPlans");
            }
        }

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
                // Проверяем существование культуры
                var crop = await _context.Crops.FindAsync(Guid.Parse(request.CropId.ToString()));
                if (crop == null)
                    return NotFound("Культура не найдена");

                // Проверяем существование поля
                var field = await _context.Fields.FindAsync(Guid.Parse(request.FieldId.ToString()));
                if (field == null)
                    return NotFound("Поле не найдено");

                // Проверяем существование сезона
                var season = await _context.Seasons.FindAsync(
                    Guid.Parse(request.SeasonId.ToString())
                );
                if (season == null)
                    return NotFound("Сезон не найден");

                // Определяем оптимальную дату посева на основе культуры, сезона и типа почвы
                var recommendedDate = CalculateOptimalPlantingDate(crop, season, field);

                // Создаем новый план посева
                var plantingPlan = new PlantingPlans
                {
                    Crop_id = Guid.Parse(request.CropId.ToString()),
                    Field_id = Guid.Parse(request.FieldId.ToString()),
                    Season_id = Guid.Parse(request.SeasonId.ToString()),
                    Planned_date = recommendedDate,
                    Expected_yield = 0,
                };

                // Сохраняем план в базу данных
                _context.PlantingPlans.Add(plantingPlan);
                await _context.SaveChangesAsync();

                // Формируем ответ
                var response = new PlantingPlanResponse
                {
                    PlanId = plantingPlan.Id.GetHashCode(),
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

        /// <summary>
        /// Рассчитывает оптимальную дату посева
        /// </summary>
        private DateTime CalculateOptimalPlantingDate(Crops crop, Seasons season, Fields field)
        {
            // Базовая дата начала сезона
            var baseDate = season.Start_date;

            // Корректируем дату в зависимости от культуры и типа почвы
            // Это упрощенная логика, которую можно расширить
            var daysToAdd = 0;

            // Пример логики: разные культуры имеют разные оптимальные периоды
            if (crop.Title.Contains("Пшеница"))
                daysToAdd = 5;
            else if (crop.Title.Contains("Кукуруза"))
                daysToAdd = 10;
            else if (crop.Title.Contains("Подсолнечник"))
                daysToAdd = 15;

            return baseDate.AddDays(daysToAdd);
        }

        /// <summary>
        /// Генерирует заметки для плана посева
        /// </summary>
        private string GeneratePlantingNotes(Crops crop, Fields field)
        {
            return $"Лучший период для посева {crop.Title} на поле {field.Title}.";
        }
    }

    /// <summary>
    /// Модель запроса для генерации плана посева
    /// </summary>
    public class PlantingPlanRequest
    {
        /// <summary>
        /// Идентификатор культуры
        /// </summary>
        public int CropId { get; set; }

        /// <summary>
        /// Идентификатор поля
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Идентификатор сезона
        /// </summary>
        public int SeasonId { get; set; }
    }

    /// <summary>
    /// Модель ответа с планом посева
    /// </summary>
    public class PlantingPlanResponse
    {
        /// <summary>
        /// Идентификатор плана
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Рекомендуемая дата посева
        /// </summary>
        public string RecommendedDate { get; set; }

        /// <summary>
        /// Заметки к плану
        /// </summary>
        public string Notes { get; set; }
    }
}
