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
    [ApiController]
    [Route("api/[controller]")]
    public class ProfitAnalysisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfitAnalysisController> _logger;

        public ProfitAnalysisController(
            ApplicationDbContext context,
            ILogger<ProfitAnalysisController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CropProfitAnalysis>> GetCropProfitAnalysis(
            [FromQuery] Guid crop_id
        )
        {
            try
            {
                // Получаем культуру и связанные данные
                var crop = await _context
                    .Crops.Include(c => c.PlantingPlans)
                    .ThenInclude(p => p.Fields)
                    .Include(c => c.PlantingPlans)
                    .ThenInclude(p => p.HarvestLogs)
                    .Include(c => c.PlantingPlans)
                    .ThenInclude(p => p.FertilizationPlans)
                    .ThenInclude(f => f.Fertilizers)
                    .FirstOrDefaultAsync(c => c.Id == crop_id);

                if (crop == null)
                {
                    return NotFound($"Культура с ID {crop_id} не найдена");
                }

                // Получаем рыночные цены (в реальном приложении здесь был бы запрос к API биржи)
                var marketPrice = await GetMarketPrice(crop.Title);

                // Анализируем все посадки культуры
                var analysis = new CropProfitAnalysis
                {
                    CropId = crop.Id,
                    CropName = crop.Title,
                    MarketPrice = marketPrice,
                    TotalArea = crop.PlantingPlans?.Sum(p => p.Fields?.Area ?? 0) ?? 0,
                    TotalHarvested =
                        crop.PlantingPlans?.Where(p => p.HarvestLogs != null)
                            .Select(p => p.HarvestLogs!)
                            .Sum(h => h.Actual_yield) ?? 0,
                    AverageYield =
                        crop.PlantingPlans?.Where(p => p.HarvestLogs != null)
                            .Select(p => p.HarvestLogs!)
                            .Average(h => h.Actual_yield) ?? 0,
                    TotalCosts = CalculateTotalCosts(crop),
                    TotalRevenue = CalculateTotalRevenue(crop, marketPrice),
                    ProfitMargin = 0,
                    CostBreakdown = new CostBreakdown
                    {
                        FertilizerCosts = CalculateFertilizerCosts(crop),
                        LaborCosts = CalculateLaborCosts(crop),
                        EquipmentCosts = CalculateEquipmentCosts(crop),
                        OtherCosts = CalculateOtherCosts(crop),
                    },
                    Recommendations = new List<string>(),
                };

                // Рассчитываем рентабельность
                analysis.ProfitMargin =
                    analysis.TotalRevenue > 0
                        ? (analysis.TotalRevenue - analysis.TotalCosts)
                            / analysis.TotalRevenue
                            * 100
                        : 0;

                // Генерируем рекомендации
                analysis.Recommendations = GenerateRecommendations(analysis);

                return Ok(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при анализе рентабельности культуры");
                return StatusCode(500, "Ошибка при анализе рентабельности культуры");
            }
        }

        private async Task<decimal> GetMarketPrice(string cropName)
        {
            // В реальном приложении здесь был бы запрос к API биржи
            // Пока используем фиксированные цены для демонстрации
            return cropName.ToLower() switch
            {
                "пшеница" => 15000m,
                "кукуруза" => 12000m,
                "подсолнечник" => 25000m,
                "соя" => 30000m,
                _ => 20000m,
            };
        }

        private decimal CalculateTotalCosts(Crops crop)
        {
            return CalculateFertilizerCosts(crop)
                + CalculateLaborCosts(crop)
                + CalculateEquipmentCosts(crop)
                + CalculateOtherCosts(crop);
        }

        private decimal CalculateFertilizerCosts(Crops crop)
        {
            // Используем фиксированную цену за единицу удобрения
            const decimal fertilizerUnitPrice = 1000m;
            return crop.PlantingPlans?.SelectMany(p =>
                        p.FertilizationPlans ?? Enumerable.Empty<FertilizationPlans>()
                    )
                    .Sum(f => f.Amount * fertilizerUnitPrice) ?? 0;
        }

        private decimal CalculateLaborCosts(Crops crop)
        {
            // В реальном приложении здесь был бы расчет затрат на рабочую силу
            return (crop.PlantingPlans?.Count ?? 0) * 50000m; // Примерная стоимость
        }

        private decimal CalculateEquipmentCosts(Crops crop)
        {
            // В реальном приложении здесь был бы расчет затрат на технику
            return (crop.PlantingPlans?.Count ?? 0) * 100000m; // Примерная стоимость
        }

        private decimal CalculateOtherCosts(Crops crop)
        {
            // В реальном приложении здесь был бы расчет прочих затрат
            return (crop.PlantingPlans?.Count ?? 0) * 25000m; // Примерная стоимость
        }

        private decimal CalculateTotalRevenue(Crops crop, decimal marketPrice)
        {
            var totalYield =
                crop.PlantingPlans?.Where(p => p.HarvestLogs != null)
                    .Select(p => p.HarvestLogs!)
                    .Sum(h => h.Actual_yield) ?? 0;

            return (decimal)totalYield * marketPrice;
        }

        private List<string> GenerateRecommendations(CropProfitAnalysis analysis)
        {
            var recommendations = new List<string>();

            if (analysis.ProfitMargin < 0)
            {
                recommendations.Add(
                    "Культура убыточна. Рекомендуется пересмотреть стратегию выращивания или рассмотреть альтернативные культуры."
                );
            }
            else if (analysis.ProfitMargin < 15)
            {
                recommendations.Add(
                    "Низкая рентабельность. Рекомендуется оптимизировать затраты и повысить урожайность."
                );
            }

            if (analysis.CostBreakdown.FertilizerCosts > analysis.TotalCosts * 0.4m)
            {
                recommendations.Add(
                    "Высокие затраты на удобрения. Рекомендуется оптимизировать использование удобрений."
                );
            }

            if (analysis.AverageYield < 3.0)
            {
                recommendations.Add(
                    "Низкая средняя урожайность. Рекомендуется улучшить агротехнику и проверить качество семян."
                );
            }

            return recommendations;
        }
    }

    public class CropProfitAnalysis
    {
        public Guid CropId { get; set; }
        public string CropName { get; set; } = string.Empty;
        public decimal MarketPrice { get; set; }
        public double TotalArea { get; set; }
        public double TotalHarvested { get; set; }
        public double AverageYield { get; set; }
        public decimal TotalCosts { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal ProfitMargin { get; set; }
        public CostBreakdown CostBreakdown { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }

    public class CostBreakdown
    {
        public decimal FertilizerCosts { get; set; }
        public decimal LaborCosts { get; set; }
        public decimal EquipmentCosts { get; set; }
        public decimal OtherCosts { get; set; }
    }
}
