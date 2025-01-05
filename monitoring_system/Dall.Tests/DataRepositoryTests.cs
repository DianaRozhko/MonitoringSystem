using DAL.EF;
using DAL.EF.Impl;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DAL.Tests
{
    public class DataRepositoryTests
    {
        private DbContextOptions<DatabaseContext> GetInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid())
                .Options;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllData()
        {
            var options = GetInMemoryDatabaseOptions();
            using (var context = new DatabaseContext(options))
            {
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

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

                await context.SaveChangesAsync();
            }

            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context);
                var data = await repository.GetAllAsync();

                Assert.Equal(2, data.Count());
                Assert.Contains(data, d => d.Id == 1 && d.Value == 25.5);
                Assert.Contains(data, d => d.Id == 2 && d.Value == 30.2);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectData()
        {
            var options = GetInMemoryDatabaseOptions();
            using (var context = new DatabaseContext(options))
            {
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

                context.Data.Add(new Data
                {
                    Id = 1,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 1,
                    Value = 25.5,
                    MeasurementType = "Celsius"
                });

                await context.SaveChangesAsync();
            }

            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context);
                var data = await repository.GetByIdAsync(1);

                Assert.NotNull(data);
                Assert.Equal(1, data.Id);
                Assert.Equal(25.5, data.Value);
            }
        }

        [Fact]
        public async Task GetBySensorIdAsync_ReturnsCorrectData()
        {
            var options = GetInMemoryDatabaseOptions();
            using (var context = new DatabaseContext(options))
            {
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

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

                context.Data.Add(new Data
                {
                    Id = 3,
                    Timestamp = DateTime.UtcNow,
                    SensorId = 2,
                    Value = 15.3,
                    MeasurementType = "Celsius"
                });

                await context.SaveChangesAsync();
            }

            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context);
                var data = await repository.GetBySensorIdAsync(1);

                Assert.Equal(2, data.Count());
                Assert.Contains(data, d => d.SensorId == 1 && d.Value == 25.5);
                Assert.Contains(data, d => d.SensorId == 1 && d.Value == 30.2);
            }
        }

        [Fact]
        public async Task GetByDateRangeAsync_ReturnsCorrectData()
        {
            var options = GetInMemoryDatabaseOptions();
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = DateTime.UtcNow.AddDays(1);

            using (var context = new DatabaseContext(options))
            {
                context.Sensors.Add(new Sensor
                {
                    Id = 1,
                    Name = "Temperature Sensor",
                    Location = "Reactor Zone",
                    Status = "Active",
                    Type = "Temperature"
                });

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
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    SensorId = 1,
                    Value = 30.2,
                    MeasurementType = "Celsius"
                });

                await context.SaveChangesAsync();
            }

            using (var context = new DatabaseContext(options))
            {
                var repository = new DataRepository(context);
                var data = await repository.GetByDateRangeAsync(startDate, endDate);

                Assert.Single(data);
                Assert.Equal(25.5, data.First().Value);
            }
        }
    }
}
