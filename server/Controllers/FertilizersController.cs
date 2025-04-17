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
                var fertilizers = await _context
                    .Fertilizers.Include(f => f.FertilizationPlans)
                    .ToListAsync();

                var alerts = new List<FertilizerStockAlert>();

                foreach (var fertilizer in fertilizers)
                {
                    var currentStock = GetFertilizerQuantity(fertilizer);

                    var plannedUsage = await _context
                        .FertilizationPlans.Where(fp => fp.Fertilization_Id == fertilizer.Id)
                        .Where(fp =>
                            fp.Application_date >= DateTime.UtcNow
                            && fp.Application_date <= DateTime.UtcNow.AddMonths(1)
                        )
                        .SumAsync(fp => fp.Amount);

                    var criticalLevel = currentStock * 0.2m;

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

        private decimal GetFertilizerQuantity(Fertilizers fertilizer)
        {
            return 10.0m;
        }
    }

    public class FertilizerStockAlert
    {
        public Guid FertilizerId { get; set; }

        public string FertilizerName { get; set; }

        public decimal CurrentStock { get; set; }

        public decimal PlannedUsage { get; set; }

        public decimal CriticalLevel { get; set; }

        public string AlertType { get; set; }

        public decimal RecommendedPurchase { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
