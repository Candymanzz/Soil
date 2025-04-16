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
    /// Контроллер для управления сезонами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SeasonsController> _logger;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="logger">Логгер</param>
        public SeasonsController(ApplicationDbContext context, ILogger<SeasonsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Инициализирует все таблицы тестовыми данными
        /// </summary>
        /// <returns>Результат операции</returns>
        /// <response code="200">Данные успешно добавлены</response>
        /// <response code="500">Ошибка при добавлении данных</response>
        [HttpPost("initialize-all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> InitializeAllData()
        {
            try
            {
                _logger.LogInformation("Начало инициализации всех таблиц");

                // Проверяем, есть ли уже данные
                if (
                    await _context.Crops.AnyAsync()
                    || await _context.Fields.AnyAsync()
                    || await _context.Fertilizers.AnyAsync()
                    || await _context.Equipment.AnyAsync()
                    || await _context.Workers.AnyAsync()
                    || await _context.Seasons.AnyAsync()
                )
                {
                    _logger.LogInformation("База данных уже содержит данные");
                    return Ok("База данных уже содержит данные");
                }

                // Создаем сезоны
                var seasons = new[]
                {
                    new Seasons
                    {
                        Id = Guid.NewGuid(),
                        Title = "Весна 2024",
                        Start_date = new DateTime(2024, 3, 1),
                        End_date = new DateTime(2024, 5, 31),
                    },
                    new Seasons
                    {
                        Id = Guid.NewGuid(),
                        Title = "Лето 2024",
                        Start_date = new DateTime(2024, 6, 1),
                        End_date = new DateTime(2024, 8, 31),
                    },
                    new Seasons
                    {
                        Id = Guid.NewGuid(),
                        Title = "Осень 2024",
                        Start_date = new DateTime(2024, 9, 1),
                        End_date = new DateTime(2024, 11, 30),
                    },
                };
                await _context.Seasons.AddRangeAsync(seasons);

                // Создаем культуры
                var crops = new[]
                {
                    new Crops
                    {
                        Id = Guid.NewGuid(),
                        Title = "Пшеница",
                        Growth_period = 120,
                        Optimal_temperature = 20,
                        Water_requirements = "Умеренные",
                    },
                    new Crops
                    {
                        Id = Guid.NewGuid(),
                        Title = "Кукуруза",
                        Growth_period = 90,
                        Optimal_temperature = 25,
                        Water_requirements = "Высокие",
                    },
                    new Crops
                    {
                        Id = Guid.NewGuid(),
                        Title = "Подсолнечник",
                        Growth_period = 100,
                        Optimal_temperature = 22,
                        Water_requirements = "Низкие",
                    },
                };
                await _context.Crops.AddRangeAsync(crops);

                // Создаем поля
                var fields = new[]
                {
                    new Fields
                    {
                        Id = Guid.NewGuid(),
                        Title = "Поле №1",
                        Area = 100.5,
                        Coordinates = "55.7558, 37.6173",
                        Soil_type = "Чернозем",
                    },
                    new Fields
                    {
                        Id = Guid.NewGuid(),
                        Title = "Поле №2",
                        Area = 150.3,
                        Coordinates = "55.7559, 37.6174",
                        Soil_type = "Супесчаная",
                    },
                    new Fields
                    {
                        Id = Guid.NewGuid(),
                        Title = "Поле №3",
                        Area = 80.7,
                        Coordinates = "55.7560, 37.6175",
                        Soil_type = "Глинистая",
                    },
                };
                await _context.Fields.AddRangeAsync(fields);

                // Создаем удобрения
                var fertilizers = new[]
                {
                    new Fertilizers
                    {
                        Id = Guid.NewGuid(),
                        Title = "Аммиачная селитра",
                        Type = "Азотное",
                        Composition = "NH4NO3",
                    },
                    new Fertilizers
                    {
                        Id = Guid.NewGuid(),
                        Title = "Суперфосфат",
                        Type = "Фосфорное",
                        Composition = "Ca(H2PO4)2",
                    },
                    new Fertilizers
                    {
                        Id = Guid.NewGuid(),
                        Title = "Калийная соль",
                        Type = "Калийное",
                        Composition = "KCl",
                    },
                };
                await _context.Fertilizers.AddRangeAsync(fertilizers);

                // Создаем технику
                var equipment = new[]
                {
                    new Equipment
                    {
                        Id = Guid.NewGuid(),
                        Title = "Трактор МТЗ-82",
                        Status = "Работоспособен",
                    },
                    new Equipment
                    {
                        Id = Guid.NewGuid(),
                        Title = "Сеялка СЗ-3.6",
                        Status = "Работоспособен",
                    },
                    new Equipment
                    {
                        Id = Guid.NewGuid(),
                        Title = "Опрыскиватель ОП-2000",
                        Status = "Работоспособен",
                    },
                };
                await _context.Equipment.AddRangeAsync(equipment);

                // Создаем работников
                var workers = new[]
                {
                    new Workers
                    {
                        Id = Guid.NewGuid(),
                        Name = "Иванов Иван",
                        Position = "Тракторист",
                        Contact = "+7 (999) 123-45-67",
                    },
                    new Workers
                    {
                        Id = Guid.NewGuid(),
                        Name = "Петров Петр",
                        Position = "Агроном",
                        Contact = "+7 (999) 234-56-78",
                    },
                    new Workers
                    {
                        Id = Guid.NewGuid(),
                        Name = "Сидоров Сидор",
                        Position = "Механик",
                        Contact = "+7 (999) 345-67-89",
                    },
                };
                await _context.Workers.AddRangeAsync(workers);

                // Сохраняем изменения для получения ID
                await _context.SaveChangesAsync();

                // Создаем планы посадки
                var plantingPlans = new[]
                {
                    new PlantingPlans
                    {
                        Id = Guid.NewGuid(),
                        Crop_id = crops[0].Id,
                        Field_id = fields[0].Id,
                        Season_id = seasons[0].Id,
                        Planned_date = new DateTime(2024, 3, 15),
                        Expected_yield = 45.5,
                    },
                    new PlantingPlans
                    {
                        Id = Guid.NewGuid(),
                        Crop_id = crops[1].Id,
                        Field_id = fields[1].Id,
                        Season_id = seasons[0].Id,
                        Planned_date = new DateTime(2024, 4, 1),
                        Expected_yield = 60.0,
                    },
                };
                await _context.PlantingPlans.AddRangeAsync(plantingPlans);

                // Сохраняем изменения для получения ID
                await _context.SaveChangesAsync();

                // Создаем планы удобрения
                var fertilizationPlans = new[]
                {
                    new FertilizationPlans
                    {
                        Id = Guid.NewGuid(),
                        Fertilization_Id = fertilizers[0].Id,
                        Plan_Id = plantingPlans[0].Id,
                        Application_date = new DateTime(2024, 3, 10),
                        Amount = 100,
                    },
                    new FertilizationPlans
                    {
                        Id = Guid.NewGuid(),
                        Fertilization_Id = fertilizers[1].Id,
                        Plan_Id = plantingPlans[1].Id,
                        Application_date = new DateTime(2024, 3, 25),
                        Amount = 150,
                    },
                };
                await _context.FertilizationPlans.AddRangeAsync(fertilizationPlans);

                // Создаем задачи
                var tasks = new[]
                {
                    new Tasks
                    {
                        Id = Guid.NewGuid(),
                        Description = "Вспашка поля №1",
                        Start_date = new DateTime(2024, 3, 1),
                        End_date = new DateTime(2024, 3, 5),
                        Status = "Запланировано",
                    },
                    new Tasks
                    {
                        Id = Guid.NewGuid(),
                        Description = "Посев пшеницы на поле №1",
                        Start_date = new DateTime(2024, 3, 15),
                        End_date = new DateTime(2024, 3, 20),
                        Status = "Запланировано",
                    },
                };
                await _context.Tasks.AddRangeAsync(tasks);

                // Создаем журналы урожая
                var harvestLogs = new[]
                {
                    new HarvestLogs
                    {
                        Id = Guid.NewGuid(),
                        Plan_Id = plantingPlans[0].Id,
                        Harvest_date = new DateTime(2024, 7, 15),
                        Actual_yield = 48.2,
                        Quality_rating = 4,
                    },
                };
                await _context.HarvestLogs.AddRangeAsync(harvestLogs);

                // Сохраняем все изменения
                await _context.SaveChangesAsync();

                _logger.LogInformation("Все таблицы успешно инициализированы");
                return Ok("Все таблицы успешно инициализированы");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при инициализации данных: {Message}", ex.Message);
                return StatusCode(500, $"Ошибка при инициализации данных: {ex.Message}");
            }
        }

        /// <summary>
        /// Инициализирует таблицу Seasons тестовыми данными
        /// </summary>
        /// <returns>Результат операции</returns>
        /// <response code="200">Данные успешно добавлены</response>
        /// <response code="500">Ошибка при добавлении данных</response>
        [HttpPost("initialize")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> InitializeSeasonsData()
        {
            try
            {
                _logger.LogInformation("Начало инициализации таблицы Seasons");

                // Проверяем, есть ли уже данные
                var existingCount = await _context.Seasons.CountAsync();
                _logger.LogInformation(
                    "Текущее количество записей в таблице Seasons: {Count}",
                    existingCount
                );

                if (existingCount > 0)
                {
                    _logger.LogInformation("Таблица Seasons уже содержит данные");
                    return Ok("Таблица Seasons уже содержит данные");
                }

                _logger.LogInformation("Создание тестовых данных для таблицы Seasons");

                // Добавляем сезоны
                var seasons = new Seasons[]
                {
                    new Seasons
                    {
                        Id = Guid.NewGuid(),
                        Title = "Весна 2023",
                        Start_date = new DateTime(2023, 3, 1),
                        End_date = new DateTime(2023, 5, 31),
                    },
                    new Seasons
                    {
                        Id = Guid.NewGuid(),
                        Title = "Лето 2023",
                        Start_date = new DateTime(2023, 6, 1),
                        End_date = new DateTime(2023, 8, 31),
                    },
                    new Seasons
                    {
                        Id = Guid.NewGuid(),
                        Title = "Осень 2023",
                        Start_date = new DateTime(2023, 9, 1),
                        End_date = new DateTime(2023, 11, 30),
                    },
                    new Seasons
                    {
                        Id = Guid.NewGuid(),
                        Title = "Зима 2023-2024",
                        Start_date = new DateTime(2023, 12, 1),
                        End_date = new DateTime(2024, 2, 29),
                    },
                };

                _logger.LogInformation(
                    "Добавление {Count} записей в таблицу Seasons",
                    seasons.Length
                );
                await _context.Seasons.AddRangeAsync(seasons);

                _logger.LogInformation("Сохранение изменений в базе данных");
                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("Сохранено {Count} записей", result);

                return Ok("Данные успешно добавлены в таблицу Seasons");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ошибка при инициализации данных в таблице Seasons: {Message}",
                    ex.Message
                );
                return StatusCode(
                    500,
                    $"Ошибка при инициализации данных в таблице Seasons: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Добавляет один сезон в таблицу Seasons
        /// </summary>
        /// <returns>Результат операции</returns>
        /// <response code="200">Сезон успешно добавлен</response>
        /// <response code="500">Ошибка при добавлении сезона</response>
        [HttpPost("add-one")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> AddOneSeason()
        {
            try
            {
                _logger.LogInformation("Добавление одного сезона в таблицу Seasons");

                var season = new Seasons
                {
                    Id = Guid.NewGuid(),
                    Title = "Весна 2023",
                    Start_date = new DateTime(2023, 3, 1),
                    End_date = new DateTime(2023, 5, 31),
                };

                _logger.LogInformation("Добавление сезона: {Title}", season.Title);
                await _context.Seasons.AddAsync(season);

                _logger.LogInformation("Сохранение изменений в базе данных");
                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("Сохранено {Count} записей", result);

                return Ok($"Сезон '{season.Title}' успешно добавлен в таблицу Seasons");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении сезона: {Message}", ex.Message);
                return StatusCode(500, $"Ошибка при добавлении сезона: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавляет один сезон в таблицу Seasons с использованием прямого SQL-запроса
        /// </summary>
        /// <returns>Результат операции</returns>
        /// <response code="200">Сезон успешно добавлен</response>
        /// <response code="500">Ошибка при добавлении сезона</response>
        [HttpPost("add-one-sql")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> AddOneSeasonWithSql()
        {
            try
            {
                _logger.LogInformation(
                    "Добавление одного сезона в таблицу Seasons с использованием SQL"
                );

                var id = Guid.NewGuid();
                var title = "Весна 2023";
                var startDate = new DateTime(2023, 3, 1);
                var endDate = new DateTime(2023, 5, 31);

                var sql =
                    @"
                    INSERT INTO ""Seasons"" (""Id"", ""Title"", ""Start_date"", ""End_date"")
                    VALUES (@id, @title, @startDate, @endDate)";

                _logger.LogInformation("Выполнение SQL-запроса: {Sql}", sql);
                var result = await _context.Database.ExecuteSqlRawAsync(
                    sql,
                    id,
                    title,
                    startDate,
                    endDate
                );

                _logger.LogInformation("SQL-запрос выполнен, затронуто строк: {Count}", result);

                return Ok(
                    $"Сезон '{title}' успешно добавлен в таблицу Seasons с использованием SQL"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ошибка при добавлении сезона с использованием SQL: {Message}",
                    ex.Message
                );
                return StatusCode(
                    500,
                    $"Ошибка при добавлении сезона с использованием SQL: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Получает список всех сезонов
        /// </summary>
        /// <returns>Список сезонов</returns>
        /// <response code="200">Возвращает список сезонов</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Seasons>>> GetSeasons()
        {
            try
            {
                _logger.LogInformation("Получение списка всех сезонов");
                var seasons = await _context.Seasons.ToListAsync();
                _logger.LogInformation("Найдено {Count} сезонов", seasons.Count);
                return Ok(seasons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка сезонов: {Message}", ex.Message);
                return StatusCode(500, $"Ошибка при получении списка сезонов: {ex.Message}");
            }
        }

        /// <summary>
        /// Проверяет подключение к базе данных и структуру таблицы Seasons
        /// </summary>
        /// <returns>Информация о подключении и структуре таблицы</returns>
        /// <response code="200">Возвращает информацию о подключении и структуре таблицы</response>
        /// <response code="500">Ошибка при проверке подключения</response>
        [HttpGet("check-db")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<object>> CheckDatabaseConnection()
        {
            try
            {
                _logger.LogInformation("Проверка подключения к базе данных");

                // Проверяем подключение к базе данных
                var canConnect = await _context.Database.CanConnectAsync();
                _logger.LogInformation("Подключение к базе данных: {CanConnect}", canConnect);

                // Получаем информацию о таблице Seasons
                var tableInfo = new
                {
                    TableName = "Seasons",
                    ColumnCount = 4, // Id, Title, Start_date, End_date
                    RowCount = await _context.Seasons.CountAsync(),
                    CanConnect = canConnect,
                    DatabaseProvider = _context.Database.ProviderName,
                    ConnectionString = _context
                        .Database.GetConnectionString()
                        ?.Replace("Password=", "Password=***"),
                };

                _logger.LogInformation("Информация о таблице Seasons: {@TableInfo}", tableInfo);

                return Ok(tableInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ошибка при проверке подключения к базе данных: {Message}",
                    ex.Message
                );
                return StatusCode(
                    500,
                    $"Ошибка при проверке подключения к базе данных: {ex.Message}"
                );
            }
        }
    }
}
