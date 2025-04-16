-- Создание сезонов
INSERT INTO
    "Seasons" ("Id", "Title", "Start_date", "End_date")
VALUES
    (
        '11111111-1111-1111-1111-111111111111',
        'Весна 2024',
        '2024-03-01 00:00:00+03',
        '2024-05-31 23:59:59+03'
    ),
    (
        '22222222-2222-2222-2222-222222222222',
        'Лето 2024',
        '2024-06-01 00:00:00+03',
        '2024-08-31 23:59:59+03'
    ),
    (
        '33333333-3333-3333-3333-333333333333',
        'Осень 2024',
        '2024-09-01 00:00:00+03',
        '2024-11-30 23:59:59+03'
    );

-- Создание культур
INSERT INTO
    "Crops" (
        "Id",
        "Title",
        "Growth_period",
        "Optimal_temperature",
        "Water_requirements"
    )
VALUES
    (
        '44444444-4444-4444-4444-444444444444',
        'Пшеница',
        120,
        20.0,
        'Умеренные'
    ),
    (
        '55555555-5555-5555-5555-555555555555',
        'Кукуруза',
        90,
        25.0,
        'Высокие'
    ),
    (
        '66666666-6666-6666-6666-666666666666',
        'Подсолнечник',
        100,
        22.0,
        'Низкие'
    );

-- Создание полей
INSERT INTO
    "Fields" (
        "Id",
        "Title",
        "Area",
        "Coordinates",
        "Soil_type"
    )
VALUES
    (
        '77777777-7777-7777-7777-777777777777',
        'Поле №1',
        100.5,
        '55.7558, 37.6173',
        'Чернозем'
    ),
    (
        '88888888-8888-8888-8888-888888888888',
        'Поле №2',
        150.3,
        '55.7559, 37.6174',
        'Супесчаная'
    ),
    (
        '99999999-9999-9999-9999-999999999999',
        'Поле №3',
        80.7,
        '55.7560, 37.6175',
        'Глинистая'
    );

-- Создание удобрений
INSERT INTO
    "Fertilizers" ("Id", "Title", "Type", "Composition")
VALUES
    (
        'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
        'Аммиачная селитра',
        'Азотное',
        'NH4NO3'
    ),
    (
        'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb',
        'Суперфосфат',
        'Фосфорное',
        'Ca(H2PO4)2'
    ),
    (
        'cccccccc-cccc-cccc-cccc-cccccccccccc',
        'Калийная соль',
        'Калийное',
        'KCl'
    );

-- Создание техники
INSERT INTO
    "Equipment" ("Id", "Title", "Type", "Status")
VALUES
    (
        'dddddddd-dddd-dddd-dddd-dddddddddddd',
        '123',
        '123',
        '123'
    ),
    (
        'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee',
        '123',
        '123',
        '123'
    ),
    (
        'ffffffff-ffff-ffff-ffff-ffffffffffff',
        '123',
        '123',
        '123'
    );

-- Создание работников
INSERT INTO
    "Workers" ("Id", "Name", "Position", "Contact")
VALUES
    (
        uuid_generate_v4(),
        'Иванов Иван',
        'Тракторист',
        '+7 (999) 123-45-67'
    ),
    (
        uuid_generate_v4(),
        'Петров Петр',
        'Агроном',
        '+7 (999) 234-56-78'
    ),
    (
        uuid_generate_v4(),
        'Сидоров Сидор',
        'Механик',
        '+7 (999) 345-67-89'
    );

-- Создание планов посадки
INSERT INTO
    "PlantingPlans" (
        "Id",
        "Crop_id",
        "Field_id",
        "Season_id",
        "Planned_date",
        "Expected_yield"
    )
VALUES
    (
        uuid_generate_v4(),
        '44444444-4444-4444-4444-444444444444',
        '77777777-7777-7777-7777-777777777777',
        '11111111-1111-1111-1111-111111111111',
        '2024-03-15 00:00:00+03',
        45.5
    ),
    (
        uuid_generate_v4(),
        '55555555-5555-5555-5555-555555555555',
        '88888888-8888-8888-8888-888888888888',
        '11111111-1111-1111-1111-111111111111',
        '2024-04-01 00:00:00+03',
        60.0
    );

-- Создание планов удобрения
INSERT INTO
    "FertilizationPlans" (
        "Id",
        "Fertilization_Id",
        "Plan_Id",
        "Application_date",
        "Amount"
    )
VALUES
    (
        uuid_generate_v4(),
        'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
        '077725e1-70b1-4752-8f08-50d79c0c5d43',
        '2024-03-10 00:00:00+03',
        100
    ),
    (
        uuid_generate_v4(),
        'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb',
        '297318e7-2f19-42cb-ad98-ffc4e51de1e2',
        '2024-03-25 00:00:00+03',
        150
    );

-- Создание задач
INSERT INTO
    "Tasks" (
        "Id",
        "Description",
        "Start_date",
        "End_date",
        "Status"
    )
VALUES
    (
        'nnnnnnnn-nnnn-nnnn-nnnn-nnnnnnnnnnnn',
        'Вспашка поля №1',
        '2024-03-01 00:00:00+03',
        '2024-03-05 23:59:59+03',
        'Запланировано'
    ),
    (
        'oooooooo-oooo-oooo-oooo-oooooooooooo',
        'Посев пшеницы на поле №1',
        '2024-03-15 00:00:00+03',
        '2024-03-20 23:59:59+03',
        'Запланировано'
    );

-- Создание журналов урожая
INSERT INTO
    "HarvestLogs" (
        "Id",
        "Plan_Id",
        "Harvest_date",
        "Actual_yield",
        "Quality_rating"
    )
VALUES
    (
        uuid_generate_v4(),
        '077725e1-70b1-4752-8f08-50d79c0c5d43',
        '2024-07-15 00:00:00+03',
        48.2,
        4
    );

-- Создание связей между техникой и задачами
INSERT INTO
    "EquipmentTasks" ("EquipmentId", "TasksId")
VALUES
    (
        'dddddddd-dddd-dddd-dddd-dddddddddddd',
        '946c061c-2f91-444d-8586-eed00605c9c4'
    ),
    (
        'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee',
        'e641dd86-f698-4171-8469-c499739ef7bd'
    );

-- Создание связей между планами посадки и задачами
INSERT INTO
    "PlantingPlansTasks" ("PlantingPlansId", "TasksId")
VALUES
    (
        '077725e1-70b1-4752-8f08-50d79c0c5d43',
        '946c061c-2f91-444d-8586-eed00605c9c4'
    ),
    (
        '297318e7-2f19-42cb-ad98-ffc4e51de1e2',
        'e641dd86-f698-4171-8469-c499739ef7bd'
    );

-- Создание связей между задачами и работниками
INSERT INTO
    "TasksWorkers" ("TasksId", "WorkersId")
VALUES
    (
        '946c061c-2f91-444d-8586-eed00605c9c4',
        '05be1357-ec08-46af-b94d-fb4aa7c2de49'
    ),
    (
        'e641dd86-f698-4171-8469-c499739ef7bd',
        'ccc2fc8b-5b5d-4c4f-b815-51bf530123e4'
    );