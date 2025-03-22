using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Проверяем, есть ли уже данные
            if (context.Users.Any())
            {
                return; // База данных уже содержит данные
            }

            // Создаем пользователей
            var users = new User[]
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin",
                    FirstName = "Admin",
                    LastName = "User"
                },
                new User
                {
                    Username = "user",
                    Email = "user@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    Role = "User",
                    FirstName = "Regular",
                    LastName = "User"
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Создаем типы почв
            var soilTypes = new SoilType[]
            {
                new SoilType
                {
                    Name = "Чернозем",
                    Description = "Плодородная почва с высоким содержанием гумуса",
                    pH = 6.5M,
                    OrganicMatter = 5.0M
                },
                new SoilType
                {
                    Name = "Суглинок",
                    Description = "Средняя по механическому составу почва",
                    pH = 6.0M,
                    OrganicMatter = 3.0M
                },
                new SoilType
                {
                    Name = "Песчаная",
                    Description = "Легкая почва с хорошей водопроницаемостью",
                    pH = 5.5M,
                    OrganicMatter = 1.5M
                }
            };

            context.SoilTypes.AddRange(soilTypes);
            context.SaveChanges();

            // Создаем культуры
            var crops = new Crop[]
            {
                new Crop
                {
                    Name = "Пшеница",
                    ScientificName = "Triticum aestivum",
                    GrowingSeason = 1,
                    AverageYield = 3.5M,
                    Description = "Основная зерновая культура"
                },
                new Crop
                {
                    Name = "Кукуруза",
                    ScientificName = "Zea mays",
                    GrowingSeason = 2,
                    AverageYield = 6.0M,
                    Description = "Высокопродуктивная зерновая культура"
                },
                new Crop
                {
                    Name = "Подсолнечник",
                    ScientificName = "Helianthus annuus",
                    GrowingSeason = 2,
                    AverageYield = 2.5M,
                    Description = "Масличная культура"
                }
            };

            context.Crops.AddRange(crops);
            context.SaveChanges();

            // Создаем удобрения
            var fertilizers = new Fertilizer[]
            {
                new Fertilizer
                {
                    Name = "Аммиачная селитра",
                    Type = "Азотное",
                    NitrogenContent = 34M,
                    PhosphorusContent = 0M,
                    PotassiumContent = 0M,
                    Unit = "кг/га",
                    PricePerUnit = 25000M,
                    Description = "Высококонцентрированное азотное удобрение"
                },
                new Fertilizer
                {
                    Name = "Суперфосфат",
                    Type = "Фосфорное",
                    NitrogenContent = 0M,
                    PhosphorusContent = 20M,
                    PotassiumContent = 0M,
                    Unit = "кг/га",
                    PricePerUnit = 30000M,
                    Description = "Фосфорное удобрение с кальцием"
                },
                new Fertilizer
                {
                    Name = "Калийная соль",
                    Type = "Калийное",
                    NitrogenContent = 0M,
                    PhosphorusContent = 0M,
                    PotassiumContent = 60M,
                    Unit = "кг/га",
                    PricePerUnit = 28000M,
                    Description = "Калийное удобрение"
                }
            };

            context.Fertilizers.AddRange(fertilizers);
            context.SaveChanges();

            // Создаем поля
            var fields = new Field[]
            {
                new Field
                {
                    Name = "Поле 1",
                    Area = 100M,
                    SoilTypeId = soilTypes[0].Id,
                    Location = "Южный участок",
                    Description = "Основное поле для пшеницы"
                },
                new Field
                {
                    Name = "Поле 2",
                    Area = 150M,
                    SoilTypeId = soilTypes[1].Id,
                    Location = "Северный участок",
                    Description = "Поле для кукурузы"
                },
                new Field
                {
                    Name = "Поле 3",
                    Area = 80M,
                    SoilTypeId = soilTypes[2].Id,
                    Location = "Западный участок",
                    Description = "Поле для подсолнечника"
                }
            };

            context.Fields.AddRange(fields);
            context.SaveChanges();

            // Создаем технику
            var equipment = new Equipment[]
            {
                new Equipment
                {
                    Name = "Трактор МТЗ-82",
                    Type = "Трактор",
                    Model = "МТЗ-82",
                    PurchaseDate = DateTime.Now.AddYears(-5),
                    PurchasePrice = 2500000M,
                    SerialNumber = "TRK-001",
                    Status = "В работе",
                    Description = "Основной трактор"
                },
                new Equipment
                {
                    Name = "Разбрасыватель удобрений",
                    Type = "Разбрасыватель",
                    Model = "РУ-2000",
                    PurchaseDate = DateTime.Now.AddYears(-3),
                    PurchasePrice = 800000M,
                    SerialNumber = "SPR-001",
                    Status = "В работе",
                    Description = "Для внесения минеральных удобрений"
                },
                new Equipment
                {
                    Name = "Плуг",
                    Type = "Почвообрабатывающее",
                    Model = "ПЛ-5-35",
                    PurchaseDate = DateTime.Now.AddYears(-4),
                    PurchasePrice = 400000M,
                    SerialNumber = "PLW-001",
                    Status = "В работе",
                    Description = "Для основной обработки почвы"
                }
            };

            context.Equipment.AddRange(equipment);
            context.SaveChanges();
        }
    }
}