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
    public class SensorRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;

        public SensorRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Sensor")
                .Options;
        }

        [Fact]
        public async Task GetAllSensors_ReturnsAllSensors()
        {
            using (var context = new DatabaseContext(_dbContextOptions))
            {
                var repository = new SensorRepository(context);
                await SeedDatabaseAsync(context);

                var result = await repository.GetAllSensorsAsync();
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetSensorById_ExistingId_ReturnsCorrectSensor()
        {
            using (var context = new DatabaseContext(_dbContextOptions))
            {
                var repository = new SensorRepository(context);
                await SeedDatabaseAsync(context);

                var result = await repository.GetSensorByIdAsync(1);
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Air Quality", result.Type);
            }
        }

        [Fact]
        public async Task GetSensorById_NonExistingId_ReturnsNull()
        {
            using (var context = new DatabaseContext(_dbContextOptions))
            {
                var repository = new SensorRepository(context);

                var result = await repository.GetSensorByIdAsync(99);
                Assert.Null(result);
            }
        }

        private async Task SeedDatabaseAsync(DatabaseContext context)
        {
            if (!context.Sensors.Any())
            {
                context.Sensors.AddRange(
                    new Sensor { Id = 1, Type = "Air Quality", Location = "Zone A", Status = "Active", Name = "Sensor A" },
                    new Sensor { Id = 2, Type = "Radiation", Location = "Zone B", Status = "Active", Name = "Sensor B" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
