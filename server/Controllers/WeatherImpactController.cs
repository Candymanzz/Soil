using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.AppDbContext;
using server.Models;

namespace server.Controllers
{
    /// <summary>
    /// Контроллер для анализа влияния погоды на посевы
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherImpactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WeatherImpactController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WeatherImpactController(
            ApplicationDbContext context,
            ILogger<WeatherImpactController> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _context = context;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// Проверяет благоприятность погоды для посева на указанную дату
        /// </summary>
        /// <param name="field_id">ID поля</param>
        /// <param name="date">Дата для проверки</param>
        /// <returns>Анализ влияния погоды на посевы</returns>
        /// <response code="200">Возвращает анализ влияния погоды</response>
        /// <response code="404">Поле не найдено</response>
        /// <response code="500">Ошибка при получении данных о погоде</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<WeatherImpactResponse>> GetWeatherImpact(
            Guid field_id,
            DateTime date
        )
        {
            // Проверяем существование поля
            var field = await _context.Fields.FindAsync(field_id);
            if (field == null)
            {
                return NotFound($"Поле с ID {field_id} не найдено");
            }

            // Получаем план посадки на указанную дату
            var plantingPlan = await _context
                .PlantingPlans.Include(p => p.Crops)
                .FirstOrDefaultAsync(p =>
                    p.Field_id == field_id && p.Planned_date.Date == date.Date
                );

            if (plantingPlan == null || plantingPlan.Crops == null)
            {
                return NotFound(
                    $"План посадки на поле с ID {field_id} на дату {date:yyyy-MM-dd} не найден"
                );
            }

            var crop = plantingPlan.Crops;

            try
            {
                // Получаем прогноз погоды из внешнего API
                var weatherData = await GetWeatherForecast(field.Coordinates, date);

                // Анализируем влияние погоды на посевы
                var impact = AnalyzeWeatherImpact(weatherData, crop);

                var response = new WeatherImpactResponse
                {
                    FieldId = field_id,
                    FieldName = field.Title,
                    Date = date,
                    CropName = crop.Title,
                    WeatherConditions = new WeatherConditions
                    {
                        Temperature = weatherData.Temperature,
                        Precipitation = weatherData.Precipitation,
                        Humidity = weatherData.Humidity,
                        WindSpeed = weatherData.WindSpeed,
                    },
                    Impact = impact,
                    Recommendations = GenerateRecommendations(impact, weatherData, crop),
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении данных о погоде");
                return StatusCode(500, "Ошибка при получении данных о погоде");
            }
        }

        private async Task<WeatherData> GetWeatherForecast(string coordinates, DateTime date)
        {
            // В реальном приложении здесь будет запрос к API погоды
            // Для демонстрации возвращаем тестовые данные
            return new WeatherData
            {
                Temperature = 15.5,
                Precipitation = 0.0,
                Humidity = 65,
                WindSpeed = 5.2,
            };
        }

        private WeatherImpact AnalyzeWeatherImpact(WeatherData weather, Crops crop)
        {
            var impact = new WeatherImpact
            {
                OverallRating = "good",
                TemperatureImpact = "good",
                PrecipitationImpact = "good",
                HumidityImpact = "good",
                WindImpact = "good",
            };

            // Анализ температуры
            var tempDiff = Math.Abs(weather.Temperature - crop.Optimal_temperature);
            if (tempDiff > 5)
            {
                impact.TemperatureImpact = "poor";
                impact.OverallRating = "poor";
            }
            else if (tempDiff > 2)
            {
                impact.TemperatureImpact = "moderate";
                if (impact.OverallRating == "good")
                {
                    impact.OverallRating = "moderate";
                }
            }

            // Анализ осадков
            if (weather.Precipitation > 5)
            {
                impact.PrecipitationImpact = "poor";
                impact.OverallRating = "poor";
            }
            else if (weather.Precipitation > 2)
            {
                impact.PrecipitationImpact = "moderate";
                if (impact.OverallRating == "good")
                {
                    impact.OverallRating = "moderate";
                }
            }

            // Анализ влажности
            if (weather.Humidity < 40 || weather.Humidity > 80)
            {
                impact.HumidityImpact = "poor";
                impact.OverallRating = "poor";
            }
            else if (weather.Humidity < 50 || weather.Humidity > 70)
            {
                impact.HumidityImpact = "moderate";
                if (impact.OverallRating == "good")
                {
                    impact.OverallRating = "moderate";
                }
            }

            // Анализ ветра
            if (weather.WindSpeed > 10)
            {
                impact.WindImpact = "poor";
                impact.OverallRating = "poor";
            }
            else if (weather.WindSpeed > 5)
            {
                impact.WindImpact = "moderate";
                if (impact.OverallRating == "good")
                {
                    impact.OverallRating = "moderate";
                }
            }

            return impact;
        }

        private List<string> GenerateRecommendations(
            WeatherImpact impact,
            WeatherData weather,
            Crops crop
        )
        {
            var recommendations = new List<string>();

            if (impact.OverallRating == "poor")
            {
                recommendations.Add("Рекомендуется перенести посев на более благоприятную дату.");
            }

            if (impact.TemperatureImpact == "poor")
            {
                if (weather.Temperature > crop.Optimal_temperature)
                {
                    recommendations.Add(
                        "Температура слишком высокая для посева. Рекомендуется дождаться похолодания."
                    );
                }
                else
                {
                    recommendations.Add(
                        "Температура слишком низкая для посева. Рекомендуется дождаться потепления."
                    );
                }
            }

            if (impact.PrecipitationImpact == "poor")
            {
                recommendations.Add(
                    "Ожидаются сильные осадки. Рекомендуется перенести посев на сухой день."
                );
            }

            if (impact.HumidityImpact == "poor")
            {
                if (weather.Humidity < 40)
                {
                    recommendations.Add(
                        "Влажность воздуха слишком низкая. Рекомендуется дождаться повышения влажности."
                    );
                }
                else
                {
                    recommendations.Add(
                        "Влажность воздуха слишком высокая. Рекомендуется дождаться снижения влажности."
                    );
                }
            }

            if (impact.WindImpact == "poor")
            {
                recommendations.Add(
                    "Скорость ветра слишком высокая для посева. Рекомендуется дождаться безветренной погоды."
                );
            }

            if (impact.OverallRating == "good")
            {
                recommendations.Add("Погодные условия благоприятны для посева.");
            }

            return recommendations;
        }
    }

    /// <summary>
    /// Модель данных о погоде
    /// </summary>
    public class WeatherData
    {
        /// <summary>
        /// Температура (°C)
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Количество осадков (мм)
        /// </summary>
        public double Precipitation { get; set; }

        /// <summary>
        /// Влажность воздуха (%)
        /// </summary>
        public double Humidity { get; set; }

        /// <summary>
        /// Скорость ветра (м/с)
        /// </summary>
        public double WindSpeed { get; set; }
    }

    /// <summary>
    /// Модель анализа влияния погоды
    /// </summary>
    public class WeatherImpact
    {
        /// <summary>
        /// Общая оценка (good, moderate, poor)
        /// </summary>
        public string OverallRating { get; set; }

        /// <summary>
        /// Влияние температуры (good, moderate, poor)
        /// </summary>
        public string TemperatureImpact { get; set; }

        /// <summary>
        /// Влияние осадков (good, moderate, poor)
        /// </summary>
        public string PrecipitationImpact { get; set; }

        /// <summary>
        /// Влияние влажности (good, moderate, poor)
        /// </summary>
        public string HumidityImpact { get; set; }

        /// <summary>
        /// Влияние ветра (good, moderate, poor)
        /// </summary>
        public string WindImpact { get; set; }
    }

    /// <summary>
    /// Модель погодных условий
    /// </summary>
    public class WeatherConditions
    {
        /// <summary>
        /// Температура (°C)
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Количество осадков (мм)
        /// </summary>
        public double Precipitation { get; set; }

        /// <summary>
        /// Влажность воздуха (%)
        /// </summary>
        public double Humidity { get; set; }

        /// <summary>
        /// Скорость ветра (м/с)
        /// </summary>
        public double WindSpeed { get; set; }
    }

    /// <summary>
    /// Модель ответа с анализом влияния погоды
    /// </summary>
    public class WeatherImpactResponse
    {
        /// <summary>
        /// ID поля
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Название поля
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Дата анализа
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Название культуры
        /// </summary>
        public string CropName { get; set; }

        /// <summary>
        /// Погодные условия
        /// </summary>
        public WeatherConditions WeatherConditions { get; set; }

        /// <summary>
        /// Анализ влияния погоды
        /// </summary>
        public WeatherImpact Impact { get; set; }

        /// <summary>
        /// Рекомендации
        /// </summary>
        public List<string> Recommendations { get; set; }
    }
}
