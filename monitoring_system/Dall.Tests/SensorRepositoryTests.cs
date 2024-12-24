using System; // Підключення основних класів .NET
using System.Collections.Generic; // Підключення для роботи зі списками
using DAL.EF.Impl; // Підключення реалізацій репозиторіїв для сенсорів
using DAL.Entities; // Підключення для моделей сутностей (наприклад, Sensor)
using DAL.EF; // Підключення для роботи з контекстом бази даних
using Microsoft.EntityFrameworkCore; // Підключення для роботи з Entity Framework Core
using Xunit; // Підключення бібліотеки для юніт-тестування

namespace DAL.Tests
{
    // Клас для тестування репозиторію сенсорів (SensorRepository)
    public class SensorRepositoryTests
    {
        private readonly SensorRepository _sensorRepository; // Репозиторій для роботи з сенсорами
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions; // Налаштування для контексту бази даних

        // Конструктор для налаштування середовища тестування
        public SensorRepositoryTests()
        {
            // **Arrange**: Налаштовуємо опції для In-Memory бази даних для тестів
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "SensorTestDb") // Використовуємо In-memory базу для тестів
                .Options;

            // Створюємо контекст бази даних з налаштованими опціями
            var context = new DatabaseContext(_dbContextOptions);

            // Ініціалізуємо репозиторій сенсорів із контекстом
            _sensorRepository = new SensorRepository(context);

            // Заповнюємо базу даних початковими тестовими даними
            SeedDatabase(context);
        }

        // Метод для заповнення бази даних початковими даними (якщо вони ще не існують)
        private void SeedDatabase(DatabaseContext context)
        {
            // Перевіряємо, чи існують вже записи з такими ID, перед тим як додавати нові
            if (!context.Sensors.Any(s => s.Id == 1))
            {
                // Додаємо перший сенсор в базу
                context.Sensors.Add(new Sensor { Id = 1, Type = "Air Quality", Location = "Zone A", Status = "Active", Name = "Sensor A" });
            }

            if (!context.Sensors.Any(s => s.Id == 2))
            {
                // Додаємо другий сенсор в базу
                context.Sensors.Add(new Sensor { Id = 2, Type = "Radiation", Location = "Zone B", Status = "Active", Name = "Sensor B" });
            }

            // Зберігаємо зміни в базі даних
            context.SaveChanges();
        }

        // Тест для отримання всіх сенсорів з репозиторію
        [Fact]
        public void GetAllSensors_ReturnsAllSensors()
        {
            // **Act**: Отримуємо список всіх сенсорів
            var result = _sensorRepository.GetAllSensors();

            // **Assert**: Перевірка, що результат не є null
            Assert.NotNull(result);

            // Перевірка, що повертається правильна кількість сенсорів
            Assert.Equal(2, result.Count);

            // Перевірка, чи міститься сенсор з типом "Air Quality"
            Assert.Contains(result, sensor => sensor.Type == "Air Quality");

            // Перевірка, чи міститься сенсор з типом "Radiation"
            Assert.Contains(result, sensor => sensor.Type == "Radiation");
        }

        // Тест для отримання сенсора за існуючим ID
        [Fact]
        public void GetSensorById_ExistingId_ReturnsCorrectSensor()
        {
            // **Act**: Отримуємо сенсор за ID 1
            var result = _sensorRepository.GetSensorById(1);

            // **Assert**: Перевірка, що результат не є null
            Assert.NotNull(result);

            // Перевірка, що повертається сенсор з правильним ID
            Assert.Equal(1, result.Id);

            // Перевірка, що сенсор має правильний тип
            Assert.Equal("Air Quality", result.Type);
        }

        // Тест для отримання сенсора за неіснуючим ID
        [Fact]
        public void GetSensorById_NonExistingId_ReturnsNull()
        {
            // **Act**: Отримуємо сенсор за ID 99 (якого не існує)
            var result = _sensorRepository.GetSensorById(99);

            // **Assert**: Перевірка, що результат є null, оскільки сенсор з таким ID не знайдений
            Assert.Null(result);
        }
    }
}
