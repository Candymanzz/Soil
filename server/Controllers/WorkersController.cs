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
    /// Контроллер для управления работниками
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WorkersController> _logger;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="logger">Логгер</param>
        public WorkersController(ApplicationDbContext context, ILogger<WorkersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Получает список работников без назначенных задач
        /// </summary>
        /// <returns>Список работников без задач</returns>
        /// <response code="200">Возвращает список работников без задач</response>
        /// <response code="500">Ошибка при получении списка работников</response>
        [HttpGet("without-tasks")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Workers>>> GetWorkersWithoutTasks()
        {
            try
            {
                _logger.LogInformation("Получение списка работников без задач");

                var workersWithoutTasks = await _context
                    .Workers.Where(w => !w.Tasks.Any())
                    .ToListAsync();

                return Ok(workersWithoutTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка работников без задач");
                return StatusCode(500, "Ошибка при получении списка работников без задач");
            }
        }

        // /// <summary>
        // /// Получает список всех работников
        // /// </summary>
        // /// <returns>Список всех работников</returns>
        // /// <response code="200">Возвращает список всех работников</response>
        // /// <response code="500">Ошибка при получении списка работников</response>
        // [HttpGet]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(500)]
        // public async Task<ActionResult<IEnumerable<Workers>>> GetAllWorkers()
        // {
        //     try
        //     {
        //         _logger.LogInformation("Получение списка всех работников");

        //         var workers = await _context.Workers.Include(w => w.Tasks).ToListAsync();

        //         return Ok(workers);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Ошибка при получении списка работников");
        //         return StatusCode(500, "Ошибка при получении списка работников");
        //     }
        // }

        // /// <summary>
        // /// Получает работника по ID
        // /// </summary>
        // /// <param name="id">ID работника</param>
        // /// <returns>Работник</returns>
        // /// <response code="200">Возвращает работника</response>
        // /// <response code="404">Работник не найден</response>
        // /// <response code="500">Ошибка при получении работника</response>
        // [HttpGet("{id}")]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(404)]
        // [ProducesResponseType(500)]
        // public async Task<ActionResult<Workers>> GetWorker(Guid id)
        // {
        //     try
        //     {
        //         _logger.LogInformation("Получение работника с ID: {Id}", id);

        //         var worker = await _context
        //             .Workers.Include(w => w.Tasks)
        //             .FirstOrDefaultAsync(w => w.Id == id);

        //         if (worker == null)
        //         {
        //             return NotFound($"Работник с ID {id} не найден");
        //         }

        //         return Ok(worker);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Ошибка при получении работника с ID: {Id}", id);
        //         return StatusCode(500, "Ошибка при получении работника");
        //     }
        // }

        // /// <summary>
        // /// Инициализирует таблицу Workers тестовыми данными
        // /// </summary>
        // /// <returns>Результат операции</returns>
        // /// <response code="200">Данные успешно добавлены</response>
        // /// <response code="500">Ошибка при добавлении данных</response>
        // [HttpPost("initialize")]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(500)]
        // public async Task<ActionResult<string>> InitializeWorkersData()
        // {
        //     try
        //     {
        //         // Проверяем, есть ли уже данные
        //         if (await _context.Workers.AnyAsync())
        //         {
        //             return Ok("Таблица Workers уже содержит данные");
        //         }

        //         // Добавляем работников
        //         var workers = new Workers[]
        //         {
        //             new Workers
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Name = "Иванов Иван",
        //                 Position = "Тракторист",
        //                 Contact = "+7 (999) 123-45-67",
        //             },
        //             new Workers
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Name = "Петров Петр",
        //                 Position = "Механик",
        //                 Contact = "+7 (999) 234-56-78",
        //             },
        //             new Workers
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Name = "Сидоров Сидор",
        //                 Position = "Оператор комбайна",
        //                 Contact = "+7 (999) 345-67-89",
        //             },
        //         };

        //         await _context.Workers.AddRangeAsync(workers);
        //         await _context.SaveChangesAsync();

        //         return Ok("Данные успешно добавлены в таблицу Workers");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Ошибка при инициализации данных в таблице Workers");
        //         return StatusCode(500, "Ошибка при инициализации данных в таблице Workers");
        //     }
        // }

        // ... existing code ...
    }
}
