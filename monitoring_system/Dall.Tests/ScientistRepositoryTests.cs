using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using DAL.EF.Impl;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DAL.Tests
{
    public class ScientistRepositoryTests
    {
        [Fact]
        public void AddScientist_AddsNewScientistSuccessfully()
        {
            // **Arrange**: Налаштовуємо середовище тесту
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Add")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                // Створюємо тестовий об'єкт Scientist
                var scientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define",
                    Username = "jorgidef",
                    Password = "password123"
                };

                // **Act**: Додаємо Scientist у репозиторій
                repository.AddScientist(scientist);

                // **Assert**: Перевіряємо, що Scientist було додано успішно
                var retrievedScientist = repository.GetScientistByUsername("jorgidef");
                Assert.NotNull(retrievedScientist);
                Assert.Equal("Jorgi Define", retrievedScientist.Name);
            }
        }

        [Fact]
        public void AddScientist_ThrowsExceptionIfUsernameExists()
        {
            // **Arrange**
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Add_Duplicate")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

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
                    Username = "jorgidef", // Такий самий Username
                    Password = "password456"
                };

                repository.AddScientist(scientist1);

                // **Act & Assert**: Додаємо Scientist з тим самим Username і перевіряємо виключення
                var exception = Assert.Throws<Exception>(() => repository.AddScientist(scientist2));
                Assert.Equal("A scientist with this username already exists.", exception.Message);
            }
        }

        [Fact]
        public void RemoveScientist_RemovesScientistSuccessfully()
        {
            // **Arrange**
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Remove")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                var scientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define",
                    Username = "jorgidef",
                    Password = "password123"
                };

                repository.AddScientist(scientist);

                // **Act**: Видаляємо Scientist
                repository.RemoveScientist(1);

                // **Assert**: Перевіряємо, що Scientist видалено
                var retrievedScientist = repository.GetScientistById(1);
                Assert.Null(retrievedScientist);
            }
        }

        [Fact]
        public void RemoveScientist_ThrowsExceptionIfScientistNotFound()
        {
            // **Arrange**
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Remove_NotFound")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                // **Act & Assert**: Перевіряємо виключення, якщо Scientist не знайдено
                var exception = Assert.Throws<Exception>(() => repository.RemoveScientist(1));
                Assert.Equal("Scientist not found.", exception.Message);
            }
        }

        [Fact]
        public void UpdateScientist_UpdatesScientistSuccessfully()
        {
            // **Arrange**
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Update")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                var scientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define",
                    Username = "jorgidef",
                    Password = "password123"
                };

                repository.AddScientist(scientist);

                var updatedScientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define Updated",
                    Password = "newpassword"
                };

                // **Act**: Оновлюємо Scientist
                repository.UpdateScientist(updatedScientist);

                // **Assert**: Перевіряємо, що дані Scientist оновлено
                var retrievedScientist = repository.GetScientistById(1);
                Assert.NotNull(retrievedScientist);
                Assert.Equal("Jorgi Define Updated", retrievedScientist.Name);
                Assert.Equal("newpassword", retrievedScientist.Password);
            }
        }

        [Fact]
        public void UpdateScientist_ThrowsExceptionIfScientistNotFound()
        {
            // **Arrange**
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Update_NotFound")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var repository = new ScientistRepository(context);

                var updatedScientist = new Scientist
                {
                    Id = 1,
                    Name = "Jorgi Define Updated",
                    Password = "newpassword"
                };

                // **Act & Assert**: Перевіряємо виключення, якщо Scientist не знайдено
                var exception = Assert.Throws<Exception>(() => repository.UpdateScientist(updatedScientist));
                Assert.Equal("Scientist not found.", exception.Message);
            }
        }
    }
}
