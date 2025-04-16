using System;
using System.Linq;
using server.AppDbContext;
using server.Models;

namespace server.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Проверяем, есть ли уже данные
            if (context.Crops.Any())
            {
                return; // База данных уже содержит данные
            }

            // Добавляем культуры
            var crops = new Crops[]
            {
                new Crops
                {
                    Id = Guid.NewGuid(),
                    Title = "Пшеница",
                    Growth_period = 120,
                    Water_requirements = "Умеренные",
                    Optimal_temperature = 20,
                },
                new Crops
                {
                    Id = Guid.NewGuid(),
                    Title = "Кукуруза",
                    Growth_period = 150,
                    Water_requirements = "Высокие",
                    Optimal_temperature = 25,
                },
                new Crops
                {
                    Id = Guid.NewGuid(),
                    Title = "Подсолнечник",
                    Growth_period = 100,
                    Water_requirements = "Низкие",
                    Optimal_temperature = 22,
                },
            };
            context.Crops.AddRange(crops);
            context.SaveChanges();

            // Добавляем поля
            var fields = new Fields[]
            {
                new Fields
                {
                    Id = Guid.NewGuid(),
                    Title = "Поле №1",
                    Area = 100,
                    Soil_type = "Чернозем",
                    Coordinates =
                        "{\"type\":\"Polygon\",\"coordinates\":[[[30.5,50.5],[30.6,50.5],[30.6,50.6],[30.5,50.6],[30.5,50.5]]]}",
                },
                new Fields
                {
                    Id = Guid.NewGuid(),
                    Title = "Поле №2",
                    Area = 150,
                    Soil_type = "Супесчаный",
                    Coordinates =
                        "{\"type\":\"Polygon\",\"coordinates\":[[[30.7,50.7],[30.8,50.7],[30.8,50.8],[30.7,50.8],[30.7,50.7]]]}",
                },
            };
            context.Fields.AddRange(fields);
            context.SaveChanges();

            // Добавляем удобрения
            var fertilizers = new Fertilizers[]
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
            };
            context.Fertilizers.AddRange(fertilizers);
            context.SaveChanges();

            // Добавляем технику
            var equipment = new Equipment[]
            {
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    Title = "Трактор МТЗ-82",
                    Type = "Трактор",
                    Status = "Доступен",
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    Title = "Сеялка СЗ-3.6",
                    Type = "Сеялка",
                    Status = "Доступен",
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    Title = "Опрыскиватель ОП-2000",
                    Type = "Опрыскиватель",
                    Status = "В ремонте",
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    Title = "Комбайн Дон-1500",
                    Type = "Комбайн",
                    Status = "Доступен",
                },
            };
            context.Equipment.AddRange(equipment);
            context.SaveChanges();

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
            context.Workers.AddRange(workers);
            context.SaveChanges();

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
                    Planned_date = DateTime.UtcNow.AddDays(45),
                    Expected_yield = 6000,
                },
            };
            context.PlantingPlans.AddRange(plantingPlans);
            context.SaveChanges();

            // Добавляем планы удобрения
            var fertilizationPlans = new FertilizationPlans[]
            {
                new FertilizationPlans
                {
                    Id = Guid.NewGuid(),
                    Plan_Id = plantingPlans[0].Id,
                    PlantingPlans = plantingPlans[0],
                    Fertilization_Id = fertilizers[0].Id,
                    Fertilizers = fertilizers[0],
                    Application_date = DateTime.UtcNow.AddDays(35),
                    Amount = 100,
                },
                new FertilizationPlans
                {
                    Id = Guid.NewGuid(),
                    Plan_Id = plantingPlans[1].Id,
                    PlantingPlans = plantingPlans[1],
                    Fertilization_Id = fertilizers[1].Id,
                    Fertilizers = fertilizers[1],
                    Application_date = DateTime.UtcNow.AddDays(50),
                    Amount = 150,
                },
            };
            context.FertilizationPlans.AddRange(fertilizationPlans);
            context.SaveChanges();

            // Добавляем задачи
            var tasks = new Tasks[]
            {
                new Tasks
                {
                    Id = Guid.NewGuid(),
                    Description = "Вспашка поля №1",
                    Start_date = DateTime.UtcNow.AddDays(25),
                    End_date = DateTime.UtcNow.AddDays(28),
                    Status = "Запланировано",
                    PlantingPlans = new List<PlantingPlans> { plantingPlans[0] },
                    Equipment = new List<Equipment> { equipment[0] },
                    Workers = new List<Workers> { workers[0] },
                },
                new Tasks
                {
                    Id = Guid.NewGuid(),
                    Description = "Посев пшеницы на поле №1",
                    Start_date = DateTime.UtcNow.AddDays(30),
                    End_date = DateTime.UtcNow.AddDays(32),
                    Status = "Запланировано",
                    PlantingPlans = new List<PlantingPlans> { plantingPlans[0] },
                    Equipment = new List<Equipment> { equipment[0], equipment[1] },
                    Workers = new List<Workers> { workers[0], workers[1] },
                },
                new Tasks
                {
                    Id = Guid.NewGuid(),
                    Description = "Вспашка поля №2",
                    Start_date = DateTime.UtcNow.AddDays(40),
                    End_date = DateTime.UtcNow.AddDays(43),
                    Status = "Запланировано",
                    PlantingPlans = new List<PlantingPlans> { plantingPlans[1] },
                    Equipment = new List<Equipment> { equipment[0] },
                    Workers = new List<Workers> { workers[0] },
                },
                new Tasks
                {
                    Id = Guid.NewGuid(),
                    Description = "Посев кукурузы на поле №2",
                    Start_date = DateTime.UtcNow.AddDays(45),
                    End_date = DateTime.UtcNow.AddDays(47),
                    Status = "Запланировано",
                    PlantingPlans = new List<PlantingPlans> { plantingPlans[1] },
                    Equipment = new List<Equipment> { equipment[0], equipment[1] },
                    Workers = new List<Workers> { workers[0], workers[1] },
                },
            };
            context.Tasks.AddRange(tasks);
            context.SaveChanges();

            // Добавляем журналы уборки
            var harvestLogs = new HarvestLogs[]
            {
                new HarvestLogs
                {
                    Id = Guid.NewGuid(),
                    Plan_Id = plantingPlans[0].Id,
                    PlantingPlans = plantingPlans[0],
                    Harvest_date = DateTime.UtcNow.AddDays(-30),
                    Actual_yield = 4200,
                    Quality_rating = 8,
                },
                new HarvestLogs
                {
                    Id = Guid.NewGuid(),
                    Plan_Id = plantingPlans[1].Id,
                    PlantingPlans = plantingPlans[1],
                    Harvest_date = DateTime.UtcNow.AddDays(-45),
                    Actual_yield = 5800,
                    Quality_rating = 7,
                },
            };
            context.HarvestLogs.AddRange(harvestLogs);
            context.SaveChanges();
        }
    }
}
