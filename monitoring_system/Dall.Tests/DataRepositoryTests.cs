using DAL.EF;
using DAL.EF.Impl;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class DataRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllData()
    {
        // **Arrange**: Налаштування середовища для тесту
        // Створюємо in-memory базу даних для імітації реальної бази.
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // В рамках блоку using створюємо контекст і додаємо тестові дані.
        using (var context = new DatabaseContext(options))
        {
            // Додаємо об'єкт Sensor, оскільки Data має зовнішній ключ SensorId
            // і цей ключ повинен вказувати на існуючий запис у таблиці Sensors.
            context.Sensors.Add(new Sensor
            {
                Id = 1,                        // Унікальний ідентифікатор сенсора
                Name = "Temperature Sensor",   // Ім'я сенсора
                Location = "Room 101",         // Обов’язкова властивість: місцезнаходження
                Status = "Active",             // Обов’язкова властивість: статус
                Type = "Thermometer"           // Обов’язкова властивість: тип сенсора
            });

            // Додаємо кілька записів Data, пов'язаних із сенсором
            context.Data.Add(new Data
            {
                Id = 1,                        // Унікальний ідентифікатор запису Data
                Timestamp = DateTime.UtcNow,   // Час створення запису
                SensorId = 1,                  // Посилання на сенсор (зовнішній ключ)
                Value = 25.5,                  // Значення, яке виміряв сенсор
                MeasurementType = "Temperature"// Тип вимірювання
            });
            context.Data.Add(new Data
            {
                Id = 2,                        // Другий запис Data
                Timestamp = DateTime.UtcNow,   // Час створення запису
                SensorId = 1,                  // Посилання на той же сенсор
                Value = 30.2,                  // Значення, яке виміряв сенсор
                MeasurementType = "Temperature"// Тип вимірювання
            });

            // Зберігаємо зміни в in-memory базі
            await context.SaveChangesAsync();
        }

        // **Act**: Виклик методу репозиторію для перевірки
        // В рамках блоку using створюємо новий контекст для ізоляції дій тесту.
        using (var context = new DatabaseContext(options))
        {
            // Ініціалізуємо репозиторій DataRepository, який будемо тестувати.
            var repository = new DataRepository(context);

            // Викликаємо метод GetAllAsync(), який має повернути всі записи Data.
            var data = await repository.GetAllAsync();

            // **Assert**: Перевірка результатів тесту
            // Переконуємося, що метод повернув саме два записи.
            Assert.Equal(2, data.Count());

            // Перевіряємо, що обидва записи присутні у результатах.
            Assert.Contains(data, d => d.Id == 1 && d.Value == 25.5); // Перший запис
            Assert.Contains(data, d => d.Id == 2 && d.Value == 30.2); // Другий запис
        }
    }
}
