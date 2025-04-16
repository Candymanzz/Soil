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
        /// Инициализирует таблицу Workers тестовыми данными
        /// </summary>
        /// <returns>Результат операции</returns>
        /// <response code="200">Данные успешно добавлены</response>
        /// <response code="500">Ошибка при добавлении данных</response>
        [HttpPost("initialize")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> InitializeWorkersData()
        {
            try
            {
                // Проверяем, есть ли уже данные
                if (await _context.Workers.AnyAsync())
                {
                    return Ok("Таблица Workers уже содержит данные");
                }

                // Добавляем работников
                var workers = new Workers[]
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
                        Position = "Механик",
                        Contact = "+7 (999) 234-56-78",
                    },
                    new Workers
                    {
                        Id = Guid.NewGuid(),
                        Name = "Сидоров Сидор",
                        Position = "Оператор комбайна",
                        Contact = "+7 (999) 345-67-89",
                    },
                };

                await _context.Workers.AddRangeAsync(workers);
                await _context.SaveChangesAsync();

                return Ok("Данные успешно добавлены в таблицу Workers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при инициализации данных в таблице Workers");
                return StatusCode(500, "Ошибка при инициализации данных в таблице Workers");
            }
        }

        // ... existing code ...
    }
}
