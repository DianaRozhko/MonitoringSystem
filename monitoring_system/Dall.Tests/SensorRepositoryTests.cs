using System;
using System.Collections.Generic;
using DAL.EF.Impl;
using DAL.Entities;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DAL.Tests
{
    public class SensorRepositoryTests
    {
        private readonly SensorRepository _sensorRepository;
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;

        public SensorRepositoryTests()
        {
            // Arrange: Create options for an in-memory database
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "SensorTestDb") // In-memory database for testing
                .Options;

            // Create a new context using in-memory database
            var context = new DatabaseContext(_dbContextOptions);

            // Initialize SensorRepository with the context
            _sensorRepository = new SensorRepository(context);

            // Seed the in-memory database with initial test data
            SeedDatabase(context);
        }

        private void SeedDatabase(DatabaseContext context)
    {
        // Перевіряємо, чи існують вже записи з такими ID, перед тим як додавати нові
        if (!context.Sensors.Any(s => s.Id == 1))
        {
            context.Sensors.Add(new Sensor { Id = 1, Type = "Air Quality", Location = "Zone A", Status = "Active", Name = "Sensor A" });
        }

        if (!context.Sensors.Any(s => s.Id == 2))
        {
            context.Sensors.Add(new Sensor { Id = 2, Type = "Radiation", Location = "Zone B", Status = "Active", Name = "Sensor B" });
        }

        context.SaveChanges();
    }


        [Fact]
        public void GetAllSensors_ReturnsAllSensors()
        {
            // Act
            var result = _sensorRepository.GetAllSensors();

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(2, result.Count); // Ensure the correct number of sensors are returned
            Assert.Contains(result, sensor => sensor.Type == "Air Quality");
            Assert.Contains(result, sensor => sensor.Type == "Radiation");
        }

        [Fact]
        public void GetSensorById_ExistingId_ReturnsCorrectSensor()
        {
            // Act
            var result = _sensorRepository.GetSensorById(1);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(1, result.Id); // Ensure the correct sensor is returned
            Assert.Equal("Air Quality", result.Type); // Ensure the sensor has the expected type
        }

        [Fact]
        public void GetSensorById_NonExistingId_ReturnsNull()
        {
            // Act
            var result = _sensorRepository.GetSensorById(99);

            // Assert
            Assert.Null(result); // Ensure the result is null as no sensor exists with this ID
        }
    }
}
