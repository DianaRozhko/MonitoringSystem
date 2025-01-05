using System;
using System.Collections.Generic;
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
        public async Task AddScientist_AddsNewScientistSuccessfully()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Scientist_Add")
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

                await repository.AddAsync(scientist);
                await context.SaveChangesAsync();

                var retrievedScientist = await repository.GetByUsernameAsync("jorgidef");
                Assert.NotNull(retrievedScientist);
                Assert.Equal("Jorgi Define", retrievedScientist.Name);
            }
        }


        [Fact]
        public async Task RemoveScientist_RemovesScientistSuccessfully()
        {
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

                await repository.AddAsync(scientist);
                await context.SaveChangesAsync();
                await repository.DeleteAsync(1);
                await context.SaveChangesAsync();

                var retrievedScientist = await repository.GetByIdAsync(1);
                Assert.Null(retrievedScientist);
            }
        }

    
    }
}
