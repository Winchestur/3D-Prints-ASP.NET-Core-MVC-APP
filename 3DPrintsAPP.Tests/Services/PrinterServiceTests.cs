using Moq;
using Xunit;
using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;

namespace _3DPrintsAPP.Tests.Services
{
    public class PrinterServiceTests
    {
        private readonly Mock<IPrinterRepository> printerRepositoryMock;
        private readonly Mock<IPrinterOptionRepository> printerOptionRepositoryMock;
        private readonly PrinterService printerService;

        public PrinterServiceTests()
        {
            printerRepositoryMock = new Mock<IPrinterRepository>();
            printerOptionRepositoryMock = new Mock<IPrinterOptionRepository>();
            printerService = new PrinterService(
                printerRepositoryMock.Object,
                printerOptionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllPrintersAsync_ShouldReturnMappedPrinters()
        {
            var printers = new List<Printer>
            {
                new Printer
                {
                    Id = 1,
                    ModelName = "Bambu Lab P1S",
                    NozzleDiameter = 0.4m,
                    Description = "Fast printer",
                    UploadPhoto = "p1s.jpg",
                    AMS = true,
                    UploadedTime = DateTime.Now
                },
                new Printer
                {
                    Id = 2,
                    ModelName = "Ender 3",
                    NozzleDiameter = 0.6m,
                    Description = "Budget printer",
                    UploadPhoto = "ender3.jpg",
                    AMS = false,
                    UploadedTime = DateTime.Now
                }
            };

            printerRepositoryMock
                .Setup(r => r.GetAllAsync("user-1"))
                .ReturnsAsync(printers);

            var result = (await printerService.GetAllPrintersAsync("user-1")).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Bambu Lab P1S", result[0].ModelName);
            Assert.Equal("Ender 3", result[1].ModelName);
        }

        [Fact]
        public async Task CreatePrinterAsync_ShouldCreatePrinter_WhenOptionExists()
        {
            var model = new PrinterCreateFromOptionViewModel
            {
                PrinterOptionId = 5
            };

            var option = new PrinterOption
            {
                Id = 5,
                ModelName = "Bambu Lab A1",
                NozzleDiameter = 0.4m,
                Description = "Option desc",
                UploadPhoto = "a1.jpg",
                AMS = true
            };

            Printer? createdPrinter = null;

            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(option);

            printerRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Printer>()))
                .Callback<Printer>(p => createdPrinter = p)
                .Returns(Task.CompletedTask);

            await printerService.CreatePrinterAsync(model, "user-1");

            Assert.NotNull(createdPrinter);
            Assert.Equal("Bambu Lab A1", createdPrinter!.ModelName);
            Assert.Equal(0.4m, createdPrinter.NozzleDiameter);
            Assert.Equal("Option desc", createdPrinter.Description);
            Assert.Equal("a1.jpg", createdPrinter.UploadPhoto);
            Assert.True(createdPrinter.AMS);
            Assert.Equal("user-1", createdPrinter.UserId);
            Assert.Equal(5, createdPrinter.PrinterOptionId);

            printerRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Printer>()), Times.Once);
        }

        [Fact]
        public async Task CreatePrinterAsync_ShouldNotCreatePrinter_WhenOptionDoesNotExist()
        {
            var model = new PrinterCreateFromOptionViewModel
            {
                PrinterOptionId = 99
            };

            printerOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((PrinterOption?)null);

            await printerService.CreatePrinterAsync(model, "user-1");

            printerRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Printer>()), Times.Never);
        }

        [Fact]
        public async Task GetPrinterForEditAsync_ShouldReturnNull_WhenPrinterNotFound()
        {
            printerRepositoryMock
                .Setup(r => r.GetByIdAsync(1, "user-1"))
                .ReturnsAsync((Printer?)null);

            var result = await printerService.GetPrinterForEditAsync(1, "user-1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPrinterForEditAsync_ShouldReturnMappedModel_WhenPrinterExists()
        {
            var printer = new Printer
            {
                Id = 1,
                ModelName = "P1S",
                NozzleDiameter = 0.4m,
                Description = "Printer desc",
                UploadPhoto = "printer.jpg",
                AMS = true,
                UserId = "user-1"
            };

            printerRepositoryMock
                .Setup(r => r.GetByIdAsync(1, "user-1"))
                .ReturnsAsync(printer);

            var result = await printerService.GetPrinterForEditAsync(1, "user-1");

            Assert.NotNull(result);
            Assert.Equal("P1S", result!.ModelName);
            Assert.Equal(0.4m, result.NozzleDiameter);
            Assert.Equal("Printer desc", result.Description);
            Assert.Equal("printer.jpg", result.UploadPhoto);
            Assert.True(result.AMS);
        }

        [Fact]
        public async Task UpdatePrinterAsync_ShouldReturnFalse_WhenPrinterNotFound()
        {
            var model = new PrinterCreateEditViewModel
            {
                ModelName = "Updated",
                NozzleDiameter = 0.6m,
                Description = "Updated desc",
                UploadPhoto = "updated.jpg",
                AMS = false
            };

            printerRepositoryMock
                .Setup(r => r.GetByIdAsync(1, "user-1"))
                .ReturnsAsync((Printer?)null);

            var result = await printerService.UpdatePrinterAsync(1, model, "user-1");

            Assert.False(result);
            printerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdatePrinterAsync_ShouldUpdateAndSave_WhenPrinterExists()
        {
            var printer = new Printer
            {
                Id = 1,
                ModelName = "Old Name",
                NozzleDiameter = 0.4m,
                Description = "Old desc",
                UploadPhoto = "old.jpg",
                AMS = true,
                UserId = "user-1"
            };

            var model = new PrinterCreateEditViewModel
            {
                ModelName = "New Name",
                NozzleDiameter = 0.8m,
                Description = "New desc",
                UploadPhoto = "new.jpg",
                AMS = false
            };

            printerRepositoryMock
                .Setup(r => r.GetByIdAsync(1, "user-1"))
                .ReturnsAsync(printer);

            var result = await printerService.UpdatePrinterAsync(1, model, "user-1");

            Assert.True(result);
            Assert.Equal("New Name", printer.ModelName);
            Assert.Equal(0.8m, printer.NozzleDiameter);
            Assert.Equal("New desc", printer.Description);
            Assert.Equal("new.jpg", printer.UploadPhoto);
            Assert.False(printer.AMS);

            printerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPrinterByIdAsync_ShouldReturnPrinter_WhenExists()
        {
            var printer = new Printer
            {
                Id = 3,
                ModelName = "Printer 3",
                UserId = "user-1"
            };

            printerRepositoryMock
                .Setup(r => r.GetByIdAsync(3, "user-1"))
                .ReturnsAsync(printer);

            var result = await printerService.GetPrinterByIdAsync(3, "user-1");

            Assert.NotNull(result);
            Assert.Equal(3, result!.Id);
            Assert.Equal("Printer 3", result.ModelName);
        }

        [Fact]
        public async Task DeletePrinterAsync_ShouldReturnFalse_WhenPrinterNotFound()
        {
            printerRepositoryMock
                .Setup(r => r.GetByIdWithFilamentsAsync(4, "user-1"))
                .ReturnsAsync((Printer?)null);

            var result = await printerService.DeletePrinterAsync(4, "user-1");

            Assert.False(result);
            printerRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Printer>()), Times.Never);
            printerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeletePrinterAsync_ShouldDeleteAndSave_WhenPrinterExists()
        {
            var printer = new Printer
            {
                Id = 4,
                ModelName = "Delete Me",
                UserId = "user-1"
            };

            printerRepositoryMock
                .Setup(r => r.GetByIdWithFilamentsAsync(4, "user-1"))
                .ReturnsAsync(printer);

            var result = await printerService.DeletePrinterAsync(4, "user-1");

            Assert.True(result);
            printerRepositoryMock.Verify(r => r.DeleteAsync(printer), Times.Once);
            printerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPrinterDetailsAsync_ShouldReturnNull_WhenPrinterNotFound()
        {
            printerRepositoryMock
                .Setup(r => r.GetByIdAsync(5, "user-1"))
                .ReturnsAsync((Printer?)null);

            var result = await printerService.GetPrinterDetailsAsync(5, "user-1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPrinterDetailsAsync_ShouldReturnMappedPrinter_WhenPrinterExists()
        {
            var uploaded = DateTime.Now;

            var printer = new Printer
            {
                Id = 5,
                ModelName = "Details Printer",
                NozzleDiameter = 0.4m,
                Description = "Details desc",
                UploadPhoto = "details.jpg",
                AMS = true,
                UploadedTime = uploaded,
                UserId = "user-1"
            };

            printerRepositoryMock
                .Setup(r => r.GetByIdAsync(5, "user-1"))
                .ReturnsAsync(printer);

            var result = await printerService.GetPrinterDetailsAsync(5, "user-1");

            Assert.NotNull(result);
            Assert.Equal(5, result!.Id);
            Assert.Equal("Details Printer", result.ModelName);
            Assert.Equal(0.4m, result.NozzleDiameter);
            Assert.Equal("Details desc", result.Description);
            Assert.Equal("details.jpg", result.UploadPhoto);
            Assert.True(result.AMS);
            Assert.Equal(uploaded, result.UploadedTime);
        }
    }
}