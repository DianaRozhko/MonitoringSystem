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
    public class DataServiceTests
    {
        private readonly DataService _dataService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDataRepository> _dataRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public DataServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _dataRepositoryMock = new Mock<IDataRepository>();
            _mapperMock = new Mock<IMapper>();

            // Підмінимо DataRepository у UnitOfWork
            _unitOfWorkMock
                .Setup(u => u.DataRepository)
                .Returns(_dataRepositoryMock.Object);

            // Створюємо DataService з моками
            _dataService = new DataService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllDataAsync_ShouldReturnAllData()
        {
            // Arrange
            var dataEntities = new List<Data>
            {
                new Data { Id = 1, Value = 10.5 },
                new Data { Id = 2, Value = 20.1 }
            };
            var dataDtos = new List<DataDTO>
            {
                new DataDTO { Id = 1, Value = 10.5 },
                new DataDTO { Id = 2, Value = 20.1 }
            };

            // Налаштування: репозиторій повертає dataEntities
            _dataRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(dataEntities);

            // Налаштування: AutoMapper мапить List<Data> -> List<DataDTO>
            _mapperMock
                .Setup(m => m.Map<IEnumerable<DataDTO>>(dataEntities))
                .Returns(dataDtos);

            // Act
            var result = await _dataService.GetAllDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, d => d.Id == 1 && d.Value == 10.5);
            Assert.Contains(result, d => d.Id == 2 && d.Value == 20.1);
        }

        [Fact]
        public async Task GetDataByIdAsync_ShouldReturnCorrectData()
        {
            // Arrange
            int existingId = 1;
            var dataEntity = new Data { Id = existingId, Value = 99.9 };
            var dataDto = new DataDTO { Id = existingId, Value = 99.9 };

            // Репозиторій повертає знайдений об'єкт
            _dataRepositoryMock
                .Setup(r => r.GetByIdAsync(existingId))
                .ReturnsAsync(dataEntity);

            // Маппер повертає DataDTO
            _mapperMock
                .Setup(m => m.Map<DataDTO>(dataEntity))
                .Returns(dataDto);

            // Act
            var result = await _dataService.GetDataByIdAsync(existingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingId, result.Id);
            Assert.Equal(99.9, result.Value);
        }

        [Fact]
        public async Task GetDataByIdAsync_ShouldThrowKeyNotFound_WhenNotFound()
        {
            // Arrange
            int nonExistingId = 999;

            _dataRepositoryMock
                .Setup(r => r.GetByIdAsync(nonExistingId))
                .ReturnsAsync((Data)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _dataService.GetDataByIdAsync(nonExistingId));

            Assert.Contains($"Data with ID {nonExistingId} not found.", ex.Message);
        }

        [Fact]
        public async Task AddDataAsync_ShouldAddAndSave()
        {
            // Arrange
            var dataDto = new DataDTO { Id = 0, Value = 50.0 };
            var dataEntity = new Data { Id = 0, Value = 50.0 };

            // Налаштування мапера
            _mapperMock
                .Setup(m => m.Map<Data>(dataDto))
                .Returns(dataEntity);

            // Act
            await _dataService.AddDataAsync(dataDto);

            // Assert
            _dataRepositoryMock.Verify(r => r.AddAsync(dataEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateDataAsync_ShouldUpdateAndSave()
        {
            // Arrange
            var dataDto = new DataDTO { Id = 1, Value = 111.1 };
            var dataEntity = new Data { Id = 1, Value = 111.1 };

            _mapperMock
                .Setup(m => m.Map<Data>(dataDto))
                .Returns(dataEntity);

            // Act
            await _dataService.UpdateDataAsync(dataDto);

            // Assert
            _dataRepositoryMock.Verify(r => r.UpdateAsync(dataEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteDataAsync_ShouldDeleteAndSave()
        {
            // Arrange
            int dataId = 100;

            // Act
            await _dataService.DeleteDataAsync(dataId);

            // Assert
            _dataRepositoryMock.Verify(r => r.DeleteAsync(dataId), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetDataBySensorIdAsync_ShouldReturnSensorData()
        {
            // Arrange
            int sensorId = 10;
            var dataEntities = new List<Data>
            {
                new Data { Id = 1, SensorId = sensorId, Value = 55.5 },
                new Data { Id = 2, SensorId = sensorId, Value = 66.6 }
            };
            var dataDtos = new List<DataDTO>
            {
                new DataDTO { Id = 1, SensorId = sensorId, Value = 55.5 },
                new DataDTO { Id = 2, SensorId = sensorId, Value = 66.6 }
            };

            _dataRepositoryMock
                .Setup(r => r.GetBySensorIdAsync(sensorId))
                .ReturnsAsync(dataEntities);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<DataDTO>>(dataEntities))
                .Returns(dataDtos);

            // Act
            var result = await _dataService.GetDataBySensorIdAsync(sensorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, d => Assert.Equal(sensorId, d.SensorId));
        }

        [Fact]
        public async Task GetDataByDateRangeAsync_ShouldReturnDataInRange()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 10);

            var dataEntities = new List<Data>
            {
                new Data { Id = 1, Timestamp = new DateTime(2023, 1, 2), Value = 10 },
                new Data { Id = 2, Timestamp = new DateTime(2023, 1, 5), Value = 20 }
            };
            var dataDtos = new List<DataDTO>
            {
                new DataDTO { Id = 1, Timestamp = new DateTime(2023, 1, 2), Value = 10 },
                new DataDTO { Id = 2, Timestamp = new DateTime(2023, 1, 5), Value = 20 }
            };

            _dataRepositoryMock
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(dataEntities);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<DataDTO>>(dataEntities))
                .Returns(dataDtos);

            // Act
            var result = await _dataService.GetDataByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, d => Assert.InRange(d.Timestamp, startDate, endDate));
        }
    }
}
