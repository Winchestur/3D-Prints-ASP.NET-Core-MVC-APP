using Moq;
using Xunit;
using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services;
using _3DPrintsAPP.Data.Models;

namespace _3DPrintsAPP.Tests.Services
{
    public class PrinterOptionServiceTests
    {
        private readonly Mock<IPrinterOptionRepository> printerOptionRepositoryMock;
        private readonly PrinterOptionService printerOptionService;

        public PrinterOptionServiceTests()
        {
            printerOptionRepositoryMock = new Mock<IPrinterOptionRepository>();
            printerOptionService = new PrinterOptionService(printerOptionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPrinterOptions()
        {
            var options = new List<PrinterOption>
            {
                new PrinterOption
                {
                    Id = 1,
                    ModelName = "Bambu Lab P1S",
                    NozzleDiameter = 0.4m,
                    Description = "Fast corexy printer",
                    UploadPhoto = "p1s.jpg",
                    AMS = true
                },
                new PrinterOption
                {
                    Id = 2,
                    ModelName = "Creality Ender 3",
                    NozzleDiameter = 0.6m,
                    Description = "Budget bedslinger",
                    UploadPhoto = "ender3.jpg",
                    AMS = false
                }
            };

            printerOptionRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(options);

            var result = (await printerOptionService.GetAllAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Bambu Lab P1S", result[0].ModelName);
            Assert.Equal("Creality Ender 3", result[1].ModelName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOption_WhenExists()
        {
            var option = new PrinterOption
            {
                Id = 5,
                ModelName = "Bambu Lab A1",
                NozzleDiameter = 0.4m,
                Description = "A1 description",
                UploadPhoto = "a1.jpg",
                AMS = true
            };

            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(option);

            var result = await printerOptionService.GetByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal(5, result!.Id);
            Assert.Equal("Bambu Lab A1", result.ModelName);
            Assert.Equal(0.4m, result.NozzleDiameter);
            Assert.True(result.AMS);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((PrinterOption?)null);

            var result = await printerOptionService.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAddAsync_WithGivenOption()
        {
            var option = new PrinterOption
            {
                Id = 10,
                ModelName = "Prusa MK4",
                NozzleDiameter = 0.4m,
                Description = "Reliable printer",
                UploadPhoto = "mk4.jpg",
                AMS = false
            };

            await printerOptionService.CreateAsync(option);

            printerOptionRepositoryMock.Verify(r => r.AddAsync(option), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenOptionNotFound()
        {
            var model = new PrinterOption
            {
                ModelName = "Updated Printer",
                NozzleDiameter = 0.8m,
                Description = "Updated description",
                UploadPhoto = "updated.jpg",
                AMS = true
            };

            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync((PrinterOption?)null);

            var result = await printerOptionService.UpdateAsync(20, model);

            Assert.False(result);
            printerOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOptionAndSave_WhenOptionExists()
        {
            var existingOption = new PrinterOption
            {
                Id = 21,
                ModelName = "Old Printer",
                NozzleDiameter = 0.4m,
                Description = "Old description",
                UploadPhoto = "old.jpg",
                AMS = false
            };

            var model = new PrinterOption
            {
                ModelName = "New Printer",
                NozzleDiameter = 0.6m,
                Description = "New description",
                UploadPhoto = "new.jpg",
                AMS = true
            };

            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(21))
                .ReturnsAsync(existingOption);

            var result = await printerOptionService.UpdateAsync(21, model);

            Assert.True(result);
            Assert.Equal(model.ModelName, existingOption.ModelName);
            Assert.Equal(model.NozzleDiameter, existingOption.NozzleDiameter);
            Assert.Equal(model.Description, existingOption.Description);
            Assert.Equal(model.UploadPhoto, existingOption.UploadPhoto);
            Assert.Equal(model.AMS, existingOption.AMS);

            printerOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenOptionNotFound()
        {
            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(30))
                .ReturnsAsync((PrinterOption?)null);

            var result = await printerOptionService.DeleteAsync(30);

            Assert.False(result);
            printerOptionRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<PrinterOption>()), Times.Never);
            printerOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOptionAndSave_WhenOptionExists()
        {
            var option = new PrinterOption
            {
                Id = 31,
                ModelName = "Delete Me",
                NozzleDiameter = 0.4m,
                Description = "To be deleted",
                UploadPhoto = "delete.jpg",
                AMS = false
            };

            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(31))
                .ReturnsAsync(option);

            var result = await printerOptionService.DeleteAsync(31);

            Assert.True(result);
            printerOptionRepositoryMock.Verify(r => r.DeleteAsync(option), Times.Once);
            printerOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}