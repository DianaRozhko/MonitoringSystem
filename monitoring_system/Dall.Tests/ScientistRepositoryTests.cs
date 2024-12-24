using System; // Підключення основних класів .NET
using System.Collections.Generic; // Підключення для роботи зі списками
using System.Linq; // Підключення для роботи з LINQ-запитами
using System.Threading.Tasks; // Підключення для асинхронного програмування
using DAL.EF; // Підключення для роботи з контекстом бази даних через Entity Framework
using DAL.Entities; // Підключення моделей сутностей (наприклад, Scientist)
using DAL.EF.Impl; // Підключення реалізацій репозиторіїв
using Microsoft.EntityFrameworkCore; // Підключення для роботи з Entity Framework Core
using Xunit; // Підключення бібліотеки для юніт-тестування

namespace DAL.Tests
{
    // Клас для тестування репозиторію науковців (ScientistRepository)
    public class ScientistRepositoryTests
    {
        // Тест для додавання нового науковця
        [Fact]
        public void AddScientist_AddsNewScientistSuccessfully()
        {
            // **Arrange**: Налаштовуємо тестове середовище
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Add") // Використовуємо In-Memory базу для тестів
                .Options;

            using (var context = new DatabaseContext(options)) // Створюємо контекст бази даних
            {
                var repository = new ScientistRepository(context); // Створюємо репозиторій для науковців

                // Створення тестового науковця
                var scientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define",
                    Username = "jorgidef",
                    Password = "password123"
                };

                // **Act**: Додаємо науковця в репозиторій
                repository.AddScientist(scientist);

                // **Assert**: Перевірка, чи науковець був доданий правильно
                var retrievedScientist = repository.GetScientistByUsername("jorgidef");
                Assert.NotNull(retrievedScientist); // Перевірка, що науковець знайдений
                Assert.Equal("Jorgi Define", retrievedScientist.Name); // Перевірка правильності імені
            }
        }

        // Тест для додавання науковця з існуючим іменем користувача (повинно кинути виключення)
        [Fact]
        public void AddScientist_ThrowsExceptionIfUsernameExists()
        {
            // **Arrange**: Налаштовуємо тестове середовище
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Add_Duplicate") // Іменуємо базу даних для цього тесту
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                // Створення двох науковців з однаковими іменами користувачів
                var scientist1 = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define",
                    Username = "jorgidef",
                    Password = "password123"
                };

                var scientist2 = new Scientist
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Username = "jorgidef", // Повторюваний username
                    Password = "password456"
                };

                repository.AddScientist(scientist1); // Додаємо першого науковця

                // **Act & Assert**: Перевіряємо, що виключення кидається при спробі додати другого науковця з тим самим username
                var exception = Assert.Throws<Exception>(() => repository.AddScientist(scientist2));
                Assert.Equal("A scientist with this username already exists.", exception.Message); // Перевірка повідомлення виключення
            }
        }

        // Тест для видалення науковця
        [Fact]
        public void RemoveScientist_RemovesScientistSuccessfully()
        {
            // **Arrange**: Підготовка тестового середовища
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Remove") // Вказуємо базу даних для цього тесту
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                // Створення та додавання науковця до бази даних
                var scientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define",
                    Username = "jorgidef",
                    Password = "password123"
                };

                repository.AddScientist(scientist);

                // **Act**: Видаляємо науковця
                repository.RemoveScientist(1);

                // **Assert**: Перевіряємо, що науковець видалений
                var retrievedScientist = repository.GetScientistById(1);
                Assert.Null(retrievedScientist); // Перевірка, що науковець більше не існує в базі
            }
        }

        // Тест для перевірки, чи кидається виключення, якщо науковець не знайдений при видаленні
        [Fact]
        public void RemoveScientist_ThrowsExceptionIfScientistNotFound()
        {
            // **Arrange**: Підготовка тестового середовища
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Remove_NotFound") // База даних для цього тесту
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                // **Act & Assert**: Перевіряємо, що кидається виключення при спробі видалити неіснуючого науковця
                var exception = Assert.Throws<Exception>(() => repository.RemoveScientist(1));
                Assert.Equal("Scientist not found.", exception.Message); // Перевірка повідомлення виключення
            }
        }

        // Тест для оновлення даних науковця
        [Fact]
        public void UpdateScientist_UpdatesScientistSuccessfully()
        {
            // **Arrange**: Налаштовуємо середовище тесту
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Update") // База даних для тесту
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                // Створюємо науковця та додаємо його в базу
                var scientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define",
                    Username = "jorgidef",
                    Password = "password123"
                };

                repository.AddScientist(scientist);

                // Створюємо нову інформацію для оновлення
                var updatedScientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define Updated",
                    Password = "newpassword"
                };

                // **Act**: Оновлюємо науковця
                repository.UpdateScientist(updatedScientist);

                // **Assert**: Перевіряємо, що дані науковця оновлені
                var retrievedScientist = repository.GetScientistById(1);
                Assert.NotNull(retrievedScientist); // Перевірка, що науковець знайдений
                Assert.Equal("Jorgi Define Updated", retrievedScientist.Name); // Перевірка нового імені
                Assert.Equal("newpassword", retrievedScientist.Password); // Перевірка нового пароля
            }
        }

        // Тест для перевірки, чи кидається виключення, якщо науковець не знайдений при оновленні
        [Fact]
        public void UpdateScientist_ThrowsExceptionIfScientistNotFound()
        {
            // **Arrange**: Налаштовуємо тестове середовище
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Update_NotFound") // База даних для тесту
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                // Створюємо науковця для оновлення
                var updatedScientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define Updated",
                    Password = "newpassword"
                };

                // **Act & Assert**: Перевіряємо, що виключення кидається, якщо науковець не знайдений
                var exception = Assert.Throws<Exception>(() => repository.UpdateScientist(updatedScientist));
                Assert.Equal("Scientist not found.", exception.Message); // Перевірка повідомлення виключення
            }
        }
    }
}
