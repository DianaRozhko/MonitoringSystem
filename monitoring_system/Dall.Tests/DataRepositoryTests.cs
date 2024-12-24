using DAL.EF; // Підключення простору імен для Entity Framework (EF) для роботи з базою даних
using DAL.EF.Impl; // Підключення реалізації репозиторіїв
using DAL.Entities; // Підключення моделей сутностей (Data, Sensor тощо)
using Microsoft.EntityFrameworkCore; // Підключення до Entity Framework Core для роботи з базою даних
using Xunit; // Підключення бібліотеки для юніт-тестування

namespace DAL.Tests
{
    // Клас, що містить тести для DataRepository
    public class DataRepositoryTests
    {
        // Метод для отримання налаштувань для In-Memory бази даних
        private DbContextOptions<DatabaseContext> GetInMemoryDatabaseOptions()
        {
            // Створення унікальних налаштувань для кожного тесту, щоб кожен тест мав свою окрему базу даних
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid()) // Використовуємо In-Memory базу з унікальним ім'ям для кожного тесту
                .Options;
        }

        // Тест для методу GetAllAsync, що перевіряє, чи всі дані повертаються
        [Fact]
        public async Task GetAllAsync_ReturnsAllData()
        {
            // **Arrange**: Налаштовуємо тестове середовище
            var options = GetInMemoryDatabaseOptions(); // Отримуємо налаштування для In-Memory бази даних

            using (var context = new DatabaseContext(options)) // Створення контексту для доступу до бази даних
            {
                // Додавання сенсора (датчика) в базу
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

                // Додавання даних, що містять вимірювання температури
                context.Data.Add(new Data
                {
                    Id = 1,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 1,
                    Value = 25.5,
                    MeasurementType = "Celsius"
                });
                context.Data.Add(new Data
                {
                    Id = 2,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 1,
                    Value = 30.2,
                    MeasurementType = "Celsius"
                });

                await context.SaveChangesAsync(); // Збереження змін у базі даних
            }

            // **Act**: Виконання тестової операції — отримання всіх даних
            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context); // Створення репозиторію для доступу до даних
                var data = await repository.GetAllAsync(); // Виклик методу для отримання всіх даних

                // **Assert**: Перевірка правильності результатів
                Assert.Equal(2, data.Count()); // Перевірка, що повернуто два записи
                Assert.Contains(data, d => d.Id == 1 && d.Value == 25.5); // Перевірка, що дані з ID = 1 присутні з правильним значенням
                Assert.Contains(data, d => d.Id == 2 && d.Value == 30.2); // Перевірка, що дані з ID = 2 присутні з правильним значенням
            }
        }

        // Тест для методу GetByIdAsync, який має повертати правильні дані за ID
        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectData()
        {
            // **Arrange**: Налаштування тестових даних для перевірки
            var options = GetInMemoryDatabaseOptions(); // Отримуємо налаштування для бази даних
            using (var context = new DatabaseContext(options))
            {
                // Додавання сенсора
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

                // Додавання даних
                context.Data.Add(new Data
                {
                    Id = 1,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 1,
                    Value = 25.5,
                    MeasurementType = "Celsius"
                });

                await context.SaveChangesAsync(); // Збереження змін у базі даних
            }

            // **Act**: Виклик методу GetByIdAsync для отримання даних за конкретним ID
            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context); // Створення репозиторію
                var data = await repository.GetByIdAsync(1); // Викликаємо метод для отримання даних за ID = 1

                // **Assert**: Перевірка результату
                Assert.NotNull(data); // Перевірка, що дані знайдені
                Assert.Equal(1, data.Id); // Перевірка правильності ID
                Assert.Equal(25.5, data.Value); // Перевірка правильності значення
            }
        }

        // Тест для методу GetBySensorIdAsync, який має повертати дані для конкретного сенсора
        [Fact]
        public async Task GetBySensorIdAsync_ReturnsCorrectData()
        {
            // **Arrange**: Підготовка тестових даних
            var options = GetInMemoryDatabaseOptions(); // Отримуємо налаштування для бази даних
            using (var context = new DatabaseContext(options))
            {
                // Додавання сенсорів
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

                // Додавання даних для сенсора 1
                context.Data.Add(new Data
                {
                    Id = 1,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 1,
                    Value = 25.5,
                    MeasurementType = "Celsius"
                });
                context.Data.Add(new Data
                {
                    Id = 2,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 1,
                    Value = 30.2,
                    MeasurementType = "Celsius"
                });

                // Додавання даних для сенсора 2
                context.Data.Add(new Data
                {
                    Id = 3,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 2,
                    Value = 15.3,
                    MeasurementType = "Celsius"
                });

                await context.SaveChangesAsync(); // Збереження даних у базі
            }

            // **Act**: Виклик методу GetBySensorIdAsync для отримання даних сенсора з ID = 1
            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context); // Створення репозиторію
                var data = await repository.GetBySensorIdAsync(1); // Виклик методу для отримання даних сенсора 1

                // **Assert**: Перевірка, що повернуто правильні дані для сенсора з ID = 1
                Assert.Equal(2, data.Count()); // Перевірка, що отримано два записи для сенсора 1
                Assert.Contains(data, d => d.SensorId == 1 && d.Value == 25.5); // Перевірка наявності першого запису
                Assert.Contains(data, d => d.SensorId == 1 && d.Value == 30.2); // Перевірка наявності другого запису
            }
        }

        // Тест для методу GetByDateRangeAsync, що має повертати дані в межах вказаного діапазону дат
        [Fact]
        public async Task GetByDateRangeAsync_ReturnsCorrectData()
        {
            // **Arrange**: Підготовка діапазону дат для тесту
            var options = GetInMemoryDatabaseOptions(); // Отримуємо налаштування для бази даних
            var startDate = DateTime.UtcNow.AddDays(-1); // Початок діапазону — один день тому
            var endDate = DateTime.UtcNow.AddDays(1); // Кінець діапазону — через один день

            using (var context = new DatabaseContext(options))
            {
                // Додавання сенсора
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

                // Додавання даних
                context.Data.Add(new Data
                {
                    Id = 1,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 1,
                    Value = 25.5,
                    MeasurementType = "Celsius"
                });
                context.Data.Add(new Data
                {
                    Id = 2,
                    Timestamp = DateTime.UtcNow.AddDays(-2), // Цей запис не має потрапити в діапазон
                    SensorId = 1,
                    Value = 30.2,
                    MeasurementType = "Celsius"
                });

                await context.SaveChangesAsync(); // Збереження змін у базі даних
            }

            // **Act**: Виклик методу для отримання даних у межах діапазону
            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context); // Створення репозиторію
                var data = await repository.GetByDateRangeAsync(startDate, endDate); // Виклик методу для отримання даних в межах діапазону

                // **Assert**: Перевірка, що лише один запис потрапив в діапазон
                Assert.Single(data); // Перевірка, що в діапазоні лише один запис
                Assert.Equal(25.5, data.First().Value); // Перевірка значення цього запису
            }
        }
    }
}
