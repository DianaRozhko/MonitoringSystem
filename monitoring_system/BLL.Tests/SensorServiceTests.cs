using Xunit;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BLL.Services.Impl;
using BLL.Services.Interfaces;
using BLL.DTO;
using DAL.UnitOfWork;
using DAL.EF.Interfaces;
using DAL.Entities;
using AutoMapper;

namespace BLL.Tests
{
    public class SensorServiceTests
    {
        private readonly SensorService _sensorService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISensorRepository> _sensorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public SensorServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sensorRepositoryMock = new Mock<ISensorRepository>();
            _mapperMock = new Mock<IMapper>();

            // Підміняємо SensorRepository в UnitOfWork
            _unitOfWorkMock
                .Setup(u => u.SensorRepository)
                .Returns(_sensorRepositoryMock.Object);

            // Створюємо екземпляр SensorService з нашими моками
            _sensorService = new SensorService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllSensorsAsync_ShouldReturnAllSensors()
        {
            // Arrange
            var sensors = new List<Sensor>
            {
                new Sensor { Id = 1, Name = "Sensor1", Type = "Temperature" },
                new Sensor { Id = 2, Name = "Sensor2", Type = "Humidity" }
            };

            var sensorDtos = new List<SensorDTO>
            {
                new SensorDTO { Id = 1, Name = "Sensor1", Type = "Temperature" },
                new SensorDTO { Id = 2, Name = "Sensor2", Type = "Humidity" }
            };

            // Репозиторій повертає sensors
            _sensorRepositoryMock
                .Setup(r => r.GetAllSensorsAsync())
                .ReturnsAsync(sensors);

            // Мапер перетворює Sensors -> SensorDTOs
            _mapperMock
                .Setup(m => m.Map<IEnumerable<SensorDTO>>(sensors))
                .Returns(sensorDtos);

            // Act
            var result = await _sensorService.GetAllSensorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, s => s.Name == "Sensor1");
            Assert.Contains(result, s => s.Name == "Sensor2");
        }

        [Fact]
        public async Task GetSensorByIdAsync_ShouldReturnSensor()
        {
            // Arrange
            int sensorId = 10;
            var sensorEntity = new Sensor { Id = sensorId, Name = "TestSensor" };
            var sensorDto = new SensorDTO { Id = sensorId, Name = "TestSensor" };

            _sensorRepositoryMock
                .Setup(r => r.GetSensorByIdAsync(sensorId))
                .ReturnsAsync(sensorEntity);

            _mapperMock
                .Setup(m => m.Map<SensorDTO>(sensorEntity))
                .Returns(sensorDto);

            // Act
            var result = await _sensorService.GetSensorByIdAsync(sensorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sensorId, result.Id);
            Assert.Equal("TestSensor", result.Name);
        }

        [Fact]
        public async Task GetSensorByIdAsync_ShouldThrowKeyNotFound_WhenSensorNotFound()
        {
            // Arrange
            int nonExistingId = 999;
            _sensorRepositoryMock
                .Setup(r => r.GetSensorByIdAsync(nonExistingId))
                .ReturnsAsync((Sensor)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _sensorService.GetSensorByIdAsync(nonExistingId));

            Assert.Contains($"Sensor with ID {nonExistingId} not found.", ex.Message);
        }

        [Fact]
        public async Task AddSensorAsync_ShouldAddAndSave()
        {
            // Arrange
            var sensorDto = new SensorDTO { Id = 0, Name = "NewSensor", Type = "Pressure" };
            var sensorEntity = new Sensor { Id = 0, Name = "NewSensor", Type = "Pressure" };

            _mapperMock
                .Setup(m => m.Map<Sensor>(sensorDto))
                .Returns(sensorEntity);

            // Act
            await _sensorService.AddSensorAsync(sensorDto);

            // Assert
            _sensorRepositoryMock.Verify(r => r.AddAsync(sensorEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSensorAsync_ShouldUpdateAndSave()
        {
            // Arrange
            var sensorDto = new SensorDTO { Id = 1, Name = "UpdatedSensor" };
            var sensorEntity = new Sensor { Id = 1, Name = "UpdatedSensor" };

            _mapperMock
                .Setup(m => m.Map<Sensor>(sensorDto))
                .Returns(sensorEntity);

            // Act
            await _sensorService.UpdateSensorAsync(sensorDto);

            // Assert
            _sensorRepositoryMock.Verify(r => r.UpdateAsync(sensorEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteSensorAsync_ShouldDeleteAndSave()
        {
            // Arrange
            int sensorId = 123;

            // Act
            await _sensorService.DeleteSensorAsync(sensorId);

            // Assert
            _sensorRepositoryMock.Verify(r => r.DeleteAsync(sensorId), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
