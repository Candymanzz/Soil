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
    /// Контроллер для управления сельскохозяйственной техникой
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EquipmentController> _logger;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="logger">Логгер</param>
        public EquipmentController(
            ApplicationDbContext context,
            ILogger<EquipmentController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        // /// <summary>
        // /// Инициализирует таблицу Equipment тестовыми данными
        // /// </summary>
        // /// <returns>Результат операции</returns>
        // /// <response code="200">Данные успешно добавлены</response>
        // /// <response code="500">Ошибка при добавлении данных</response>
        // [HttpPost("initialize")]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(500)]
        // public async Task<ActionResult<string>> InitializeEquipmentData()
        // {
        //     try
        //     {
        //         // Проверяем, есть ли уже данные
        //         if (await _context.Equipment.AnyAsync())
        //         {
        //             return Ok("Таблица Equipment уже содержит данные");
        //         }

        //         // Добавляем технику
        //         var equipment = new Equipment[]
        //         {
        //             new Equipment
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Title = "Трактор МТЗ-82",
        //                 Type = "Трактор",
        //                 Status = "Доступен",
        //             },
        //             new Equipment
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Title = "Сеялка СЗ-3.6",
        //                 Type = "Сеялка",
        //                 Status = "Доступен",
        //             },
        //             new Equipment
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Title = "Опрыскиватель ОП-2000",
        //                 Type = "Опрыскиватель",
        //                 Status = "В ремонте",
        //             },
        //             new Equipment
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Title = "Комбайн Дон-1500",
        //                 Type = "Комбайн",
        //                 Status = "Доступен",
        //             },
        //         };

        //         await _context.Equipment.AddRangeAsync(equipment);
        //         await _context.SaveChangesAsync();

        //         return Ok("Данные успешно добавлены в таблицу Equipment");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Ошибка при инициализации данных в таблице Equipment");
        //         return StatusCode(500, "Ошибка при инициализации данных в таблице Equipment");
        //     }
        // }

        /// <summary>
        /// Проверяет доступность техники на указанную дату
        /// </summary>
        /// <param name="equipment_id">ID техники</param>
        /// <param name="date">Дата для проверки</param>
        /// <returns>Информация о доступности техники</returns>
        /// <response code="200">Возвращает информацию о доступности техники</response>
        /// <response code="400">Неверные входные данные</response>
        /// <response code="404">Техника не найдена</response>
        [HttpGet("{equipment_id}/availability")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EquipmentAvailabilityResponse>> CheckAvailability(
            Guid equipment_id,
            DateTime date
        )
        {
            var response = new EquipmentAvailabilityResponse
            {
                IsAvailable = true,
                ConflictingTasks = new List<ConflictingTaskInfo>(),
            };

            var conflictingTasks = await _context
                .Tasks.Include(t => t.Equipment)
                .Where(t =>
                    t.Equipment.Any(e => e.Id == equipment_id)
                    && t.Start_date <= date
                    && t.End_date >= date
                )
                .Select(t => new ConflictingTaskInfo
                {
                    TaskId = t.Id,
                    TaskName = t.Description,
                    PlannedDate = t.Start_date,
                    Status = t.Status,
                })
                .ToListAsync();

            if (conflictingTasks.Any())
            {
                response.IsAvailable = false;
                response.ConflictingTasks = conflictingTasks;
            }

            return Ok(response);
        }
    }

    public class EquipmentAvailabilityResponse
    {
        public bool IsAvailable { get; set; }

        public List<ConflictingTaskInfo> ConflictingTasks { get; set; }
    }

    public class ConflictingTaskInfo
    {
        public Guid TaskId { get; set; }

        public string TaskName { get; set; }

        public DateTime PlannedDate { get; set; }

        public string Status { get; set; }
    }
}
