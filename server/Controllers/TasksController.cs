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
    /// Контроллер для управления задачами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ApplicationDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // /// <summary>
        // /// Инициализирует таблицу Tasks тестовыми данными
        // /// </summary>
        // /// <returns>Результат операции</returns>
        // /// <response code="200">Данные успешно добавлены</response>
        // /// <response code="500">Ошибка при добавлении данных</response>
        // [HttpPost("initialize")]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(500)]
        // public async Task<ActionResult<string>> InitializeTasksData()
        // {
        //     try
        //     {
        //         // Проверяем, есть ли уже данные
        //         if (await _context.Tasks.AnyAsync())
        //         {
        //             return Ok("Таблица Tasks уже содержит данные");
        //         }

        //         // Получаем необходимые данные для создания задач
        //         var plantingPlans = await _context.PlantingPlans.ToListAsync();
        //         var equipment = await _context.Equipment.ToListAsync();
        //         var workers = await _context.Workers.ToListAsync();

        //         if (!plantingPlans.Any() || !equipment.Any() || !workers.Any())
        //         {
        //             return BadRequest(
        //                 "Необходимые данные для создания задач отсутствуют. Убедитесь, что таблицы PlantingPlans, Equipment и Workers содержат данные."
        //             );
        //         }

        //         // Добавляем задачи
        //         var tasks = new Tasks[]
        //         {
        //             new Tasks
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Description = "Вспашка поля №1",
        //                 Start_date = DateTime.UtcNow.AddDays(25),
        //                 End_date = DateTime.UtcNow.AddDays(28),
        //                 Status = "Запланировано",
        //                 PlantingPlans = new List<PlantingPlans> { plantingPlans[0] },
        //                 Equipment = new List<Equipment> { equipment[0] },
        //                 Workers = new List<Workers> { workers[0] },
        //             },
        //             new Tasks
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Description = "Посев пшеницы на поле №1",
        //                 Start_date = DateTime.UtcNow.AddDays(30),
        //                 End_date = DateTime.UtcNow.AddDays(32),
        //                 Status = "Запланировано",
        //                 PlantingPlans = new List<PlantingPlans> { plantingPlans[0] },
        //                 Equipment = new List<Equipment> { equipment[0], equipment[1] },
        //                 Workers = new List<Workers> { workers[0], workers[1] },
        //             },
        //             new Tasks
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Description = "Вспашка поля №2",
        //                 Start_date = DateTime.UtcNow.AddDays(40),
        //                 End_date = DateTime.UtcNow.AddDays(43),
        //                 Status = "Запланировано",
        //                 PlantingPlans = new List<PlantingPlans> { plantingPlans[1] },
        //                 Equipment = new List<Equipment> { equipment[0] },
        //                 Workers = new List<Workers> { workers[0] },
        //             },
        //             new Tasks
        //             {
        //                 Id = Guid.NewGuid(),
        //                 Description = "Посев кукурузы на поле №2",
        //                 Start_date = DateTime.UtcNow.AddDays(45),
        //                 End_date = DateTime.UtcNow.AddDays(47),
        //                 Status = "Запланировано",
        //                 PlantingPlans = new List<PlantingPlans> { plantingPlans[1] },
        //                 Equipment = new List<Equipment> { equipment[0], equipment[1] },
        //                 Workers = new List<Workers> { workers[0], workers[1] },
        //             },
        //         };

        //         await _context.Tasks.AddRangeAsync(tasks);
        //         await _context.SaveChangesAsync();

        //         return Ok("Данные успешно добавлены в таблицу Tasks");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Ошибка при инициализации данных в таблице Tasks");
        //         return StatusCode(500, "Ошибка при инициализации данных в таблице Tasks");
        //     }
        // }

        /// <summary>
        /// Создает задачу и отправляет уведомление работнику
        /// </summary>
        /// <param name="request">Данные для создания задачи</param>
        /// <returns>Созданная задача</returns>
        /// <response code="200">Задача успешно создана</response>
        /// <response code="400">Неверные входные данные</response>
        /// <response code="404">Работник не найден</response>
        /// <response code="500">Ошибка при создании задачи</response>
        [HttpPost("assign")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TaskAssignmentResponse>> AssignTask(
            TaskAssignmentRequest request
        )
        {
            try
            {
                var worker = await _context.Workers.FindAsync(request.WorkerId);
                if (worker == null)
                {
                    return NotFound($"Работник с ID {request.WorkerId} не найден");
                }

                if (request.EquipmentIds != null && request.EquipmentIds.Any())
                {
                    var equipment = await _context
                        .Equipment.Where(e => request.EquipmentIds.Contains(e.Id))
                        .ToListAsync();

                    if (equipment.Count != request.EquipmentIds.Count)
                    {
                        return BadRequest("Одно или несколько единиц оборудования не найдены");
                    }
                }

                var task = new Tasks
                {
                    Description = request.Description,
                    Start_date = request.StartDate,
                    End_date = request.EndDate,
                    Status = "Назначена",
                };

                task.Workers = new List<Workers> { worker };

                if (request.EquipmentIds != null && request.EquipmentIds.Any())
                {
                    task.Equipment = await _context
                        .Equipment.Where(e => request.EquipmentIds.Contains(e.Id))
                        .ToListAsync();
                }

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                var notification = new TaskNotification
                {
                    TaskId = task.Id,
                    WorkerId = worker.Id,
                    WorkerName = worker.Name,
                    TaskDescription = task.Description,
                    StartDate = task.Start_date,
                    EndDate = task.End_date,
                    Status = task.Status,
                    NotificationType = "Новая задача",
                    CreatedAt = DateTime.Now,
                };

                await SendNotification(notification);

                var response = new TaskAssignmentResponse
                {
                    TaskId = task.Id,
                    Description = task.Description,
                    StartDate = task.Start_date,
                    EndDate = task.End_date,
                    Status = task.Status,
                    WorkerName = worker.Name,
                    NotificationSent = true,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании задачи");
                return StatusCode(500, "Ошибка при создании задачи");
            }
        }

        private async Task SendNotification(TaskNotification notification)
        {
            _logger.LogInformation(
                "Уведомление отправлено работнику {WorkerName} о задаче {TaskId}",
                notification.WorkerName,
                notification.TaskId
            );
        }
    }

    public class TaskAssignmentRequest
    {
        public Guid WorkerId { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<Guid>? EquipmentIds { get; set; }
    }

    public class TaskAssignmentResponse
    {
        public Guid TaskId { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public string WorkerName { get; set; }

        public bool NotificationSent { get; set; }
    }

    public class TaskNotification
    {
        public Guid TaskId { get; set; }

        public Guid WorkerId { get; set; }

        public string WorkerName { get; set; }

        public string TaskDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public string NotificationType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
