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
    /// Контроллер для управления удобрениями
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FertilizersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FertilizersController> _logger;

        public FertilizersController(
            ApplicationDbContext context,
            ILogger<FertilizersController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Возвращает список удобрений с низким остатком
        /// </summary>
        /// <returns>Список удобрений, требующих пополнения</returns>
        /// <response code="200">Возвращает список удобрений с низким остатком</response>
        /// <response code="500">Ошибка при получении данных</response>
        [HttpGet("stock-alerts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<FertilizerStockAlert>>> GetStockAlerts()
        {
            try
            {
                // Получаем все удобрения
                var fertilizers = await _context
                    .Fertilizers.Include(f => f.FertilizationPlans)
                    .ToListAsync();

                var alerts = new List<FertilizerStockAlert>();

                foreach (var fertilizer in fertilizers)
                {
                    // Рассчитываем текущий остаток
                    var currentStock = GetFertilizerQuantity(fertilizer);

                    // Рассчитываем планируемое использование на ближайший месяц
                    var plannedUsage = await _context
                        .FertilizationPlans.Where(fp => fp.Fertilization_Id == fertilizer.Id)
                        .Where(fp =>
                            fp.Application_date >= DateTime.UtcNow
                            && fp.Application_date <= DateTime.UtcNow.AddMonths(1)
                        )
                        .SumAsync(fp => fp.Amount);

                    // Рассчитываем критический уровень (20% от текущего остатка)
                    var criticalLevel = currentStock * 0.2m;

                    // Если остаток меньше критического уровня или планируемое использование превышает остаток
                    if (currentStock <= criticalLevel || currentStock < plannedUsage)
                    {
                        var alert = new FertilizerStockAlert
                        {
                            FertilizerId = fertilizer.Id,
                            FertilizerName = fertilizer.Title,
                            CurrentStock = currentStock,
                            PlannedUsage = plannedUsage,
                            CriticalLevel = criticalLevel,
                            AlertType =
                                currentStock <= criticalLevel
                                    ? "Критический уровень"
                                    : "Недостаточно для планов",
                            RecommendedPurchase = Math.Max(
                                plannedUsage - currentStock,
                                criticalLevel * 2
                            ),
                            LastUpdated = DateTime.UtcNow,
                        };

                        alerts.Add(alert);
                    }
                }

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка удобрений с низким остатком");
                return StatusCode(500, "Ошибка при получении списка удобрений с низким остатком");
            }
        }

        /// <summary>
        /// Получает количество удобрения
        /// </summary>
        /// <param name="fertilizer">Удобрение</param>
        /// <returns>Количество удобрения</returns>
        private decimal GetFertilizerQuantity(Fertilizers fertilizer)
        {
            // В реальном приложении здесь будет получение количества из базы данных
            // Для демонстрации возвращаем фиксированное значение
            return 1000.0m;
        }
    }

    /// <summary>
    /// Модель оповещения о состоянии запасов удобрений
    /// </summary>
    public class FertilizerStockAlert
    {
        /// <summary>
        /// ID удобрения
        /// </summary>
        public Guid FertilizerId { get; set; }

        /// <summary>
        /// Название удобрения
        /// </summary>
        public string FertilizerName { get; set; }

        /// <summary>
        /// Текущий остаток (кг)
        /// </summary>
        public decimal CurrentStock { get; set; }

        /// <summary>
        /// Планируемое использование в ближайший месяц (кг)
        /// </summary>
        public decimal PlannedUsage { get; set; }

        /// <summary>
        /// Критический уровень (кг)
        /// </summary>
        public decimal CriticalLevel { get; set; }

        /// <summary>
        /// Тип оповещения
        /// </summary>
        public string AlertType { get; set; }

        /// <summary>
        /// Рекомендуемое количество для закупки (кг)
        /// </summary>
        public decimal RecommendedPurchase { get; set; }

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
