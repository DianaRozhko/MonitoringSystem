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
    public class ScientistServiceTests
    {
        private readonly ScientistService _scientistService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IScientistRepository> _scientistRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public ScientistServiceTests()
        {
            // Створюємо моки
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _scientistRepositoryMock = new Mock<IScientistRepository>();
            _mapperMock = new Mock<IMapper>();

            // Підміняємо ScientistRepository у UnitOfWork
            _unitOfWorkMock
                .Setup(u => u.ScientistRepository)
                .Returns(_scientistRepositoryMock.Object);

            // Створюємо екземпляр ScientistService з нашими моками
            _scientistService = new ScientistService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllScientistsAsync_ShouldReturnAllScientists()
        {
            // Arrange
            var scientistsFromDb = new List<Scientist>
            {
                new Scientist { Id = 1, Name = "Alice" },
                new Scientist { Id = 2, Name = "Bob" }
            };

            var scientistDtos = new List<ScientistDTO>
            {
                new ScientistDTO { Id = 1, Name = "Alice" },
                new ScientistDTO { Id = 2, Name = "Bob" }
            };

            // Налаштовуємо репозиторій повернути scientistsFromDb
            _scientistRepositoryMock
                .Setup(r => r.GetAllScientistsAsync())
                .ReturnsAsync(scientistsFromDb);

            // Налаштовуємо mapper, щоб він мапив List<Scientist> -> List<ScientistDTO>
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ScientistDTO>>(scientistsFromDb))
                .Returns(scientistDtos);

            // Act
            var result = await _scientistService.GetAllScientistsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, s => s.Name == "Alice");
            Assert.Contains(result, s => s.Name == "Bob");
        }

        [Fact]
        public async Task GetScientistByIdAsync_ShouldReturnScientist_WhenScientistExists()
        {
            // Arrange
            int scientistId = 1;
            var scientistFromDb = new Scientist { Id = scientistId, Name = "Alice" };
            var scientistDto = new ScientistDTO { Id = scientistId, Name = "Alice" };

            _scientistRepositoryMock
                .Setup(r => r.GetByIdAsync(scientistId))
                .ReturnsAsync(scientistFromDb);

            _mapperMock
                .Setup(m => m.Map<ScientistDTO>(scientistFromDb))
                .Returns(scientistDto);

            // Act
            var result = await _scientistService.GetScientistByIdAsync(scientistId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(scientistId, result.Id);
            Assert.Equal("Alice", result.Name);
        }

        [Fact]
        public async Task GetScientistByIdAsync_ShouldThrowKeyNotFound_WhenScientistNotFound()
        {
            // Arrange
            int nonExistingId = 999;
            _scientistRepositoryMock
                .Setup(r => r.GetByIdAsync(nonExistingId))
                .ReturnsAsync((Scientist)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _scientistService.GetScientistByIdAsync(nonExistingId));

            Assert.Contains($"Scientist with ID {nonExistingId} not found.", exception.Message);
        }

        [Fact]
        public async Task AddScientistAsync_ShouldCallAddAndSave()
        {
            // Arrange
            var scientistDto = new ScientistDTO { Id = 0, Name = "NewScientist" };
            var scientistEntity = new Scientist { Id = 0, Name = "NewScientist" };

            _mapperMock
                .Setup(m => m.Map<Scientist>(scientistDto))
                .Returns(scientistEntity);

            // Act
            await _scientistService.AddScientistAsync(scientistDto);

            // Assert
            _scientistRepositoryMock.Verify(r => r.AddAsync(scientistEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateScientistAsync_ShouldUpdateAndSave()
        {
            // Arrange
            var scientistDto = new ScientistDTO { Id = 1, Name = "UpdatedName" };
            var scientistEntity = new Scientist { Id = 1, Name = "UpdatedName" };

            _mapperMock
                .Setup(m => m.Map<Scientist>(scientistDto))
                .Returns(scientistEntity);

            // Act
            await _scientistService.UpdateScientistAsync(scientistDto);

            // Assert
            _scientistRepositoryMock.Verify(r => r.UpdateAsync(scientistEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteScientistAsync_ShouldDeleteAndSave()
        {
            // Arrange
            int scientistId = 1;

            // Act
            await _scientistService.DeleteScientistAsync(scientistId);

            // Assert
            _scientistRepositoryMock.Verify(r => r.DeleteAsync(scientistId), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
