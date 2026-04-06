using System.Security.Claims;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Controllers;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;

namespace _3DPrintsAPP.Tests.Controllers
{
    public class PrintersControllerTests
    {
        private readonly Mock<IPrinterService> printerServiceMock;
        private readonly Mock<IPrinterOptionService> printerOptionServiceMock;

        public PrintersControllerTests()
        {
            printerServiceMock = new Mock<IPrinterService>();
            printerOptionServiceMock = new Mock<IPrinterOptionService>();
        }

        private PrintersController CreateController(string userId = "user-1")
        {
            var controller = new PrintersController(
                printerServiceMock.Object,
                printerOptionServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuth"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            return controller;
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithPrinters()
        {
            var printers = new List<PrinterViewModel>
            {
                new PrinterViewModel { Id = 1, ModelName = "P1S" },
                new PrinterViewModel { Id = 2, ModelName = "Ender 3" }
            };

            printerServiceMock
                .Setup(s => s.GetAllPrintersAsync("user-1"))
                .ReturnsAsync(printers);

            var controller = CreateController();

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PrinterViewModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task CreateGet_ShouldReturnViewWithPrinterOptions()
        {
            var options = new List<PrinterOption>
            {
                new PrinterOption { Id = 1, ModelName = "P1S" },
                new PrinterOption { Id = 2, ModelName = "A1" }
            };

            printerOptionServiceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(options);

            var controller = CreateController();

            var result = await controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PrinterCreateFromOptionViewModel>(viewResult.Model);

            var printerOptions = model.PrinterOptions.ToList();
            Assert.Equal(2, printerOptions.Count);
            Assert.Equal("1", printerOptions[0].Value);
            Assert.Equal("P1S", printerOptions[0].Text);
            Assert.Equal("2", printerOptions[1].Value);
            Assert.Equal("A1", printerOptions[1].Text);
        }

        [Fact]
        public async Task CreatePost_ShouldReturnViewWithOptions_WhenModelStateIsInvalid()
        {
            var model = new PrinterCreateFromOptionViewModel
            {
                PrinterOptionId = 1
            };

            var options = new List<PrinterOption>
            {
                new PrinterOption { Id = 1, ModelName = "P1S" },
                new PrinterOption { Id = 2, ModelName = "A1" }
            };

            printerOptionServiceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(options);

            var controller = CreateController();
            controller.ModelState.AddModelError("PrinterOptionId", "Required");

            var result = await controller.Create(model);

            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedModel = Assert.IsType<PrinterCreateFromOptionViewModel>(viewResult.Model);

            var printerOptions = returnedModel.PrinterOptions.ToList();
            Assert.Equal(2, printerOptions.Count);
            Assert.Equal("P1S", printerOptions[0].Text);

            printerServiceMock.Verify(s => s.CreatePrinterAsync(It.IsAny<PrinterCreateFromOptionViewModel>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreatePost_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            var model = new PrinterCreateFromOptionViewModel
            {
                PrinterOptionId = 1
            };

            var controller = CreateController();

            var result = await controller.Create(model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            printerServiceMock.Verify(s => s.CreatePrinterAsync(model, "user-1"), Times.Once);
        }

        [Fact]
        public async Task EditGet_ShouldReturnNotFound_WhenPrinterDoesNotExist()
        {
            printerServiceMock
                .Setup(s => s.GetPrinterForEditAsync(1, "user-1"))
                .ReturnsAsync((PrinterCreateEditViewModel?)null);

            var controller = CreateController();

            var result = await controller.Edit(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ShouldReturnView_WhenPrinterExists()
        {
            var model = new PrinterCreateEditViewModel
            {
                ModelName = "P1S"
            };

            printerServiceMock
                .Setup(s => s.GetPrinterForEditAsync(1, "user-1"))
                .ReturnsAsync(model);

            var controller = CreateController();

            var result = await controller.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ShouldReturnView_WhenModelStateIsInvalid()
        {
            var model = new PrinterCreateEditViewModel
            {
                ModelName = "Bad"
            };

            var controller = CreateController();
            controller.ModelState.AddModelError("ModelName", "Required");

            var result = await controller.Edit(1, model);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);

            printerServiceMock.Verify(s => s.UpdatePrinterAsync(It.IsAny<int>(), It.IsAny<PrinterCreateEditViewModel>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFound_WhenUpdateFails()
        {
            var model = new PrinterCreateEditViewModel
            {
                ModelName = "P1S"
            };

            printerServiceMock
                .Setup(s => s.UpdatePrinterAsync(1, model, "user-1"))
                .ReturnsAsync(false);

            var controller = CreateController();

            var result = await controller.Edit(1, model);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ShouldRedirectToIndex_WhenUpdateSucceeds()
        {
            var model = new PrinterCreateEditViewModel
            {
                ModelName = "P1S"
            };

            printerServiceMock
                .Setup(s => s.UpdatePrinterAsync(1, model, "user-1"))
                .ReturnsAsync(true);

            var controller = CreateController();

            var result = await controller.Edit(1, model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnNotFound_WhenPrinterDoesNotExist()
        {
            printerServiceMock
                .Setup(s => s.GetPrinterByIdAsync(1, "user-1"))
                .ReturnsAsync((Printer?)null);

            var controller = CreateController();

            var result = await controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnView_WhenPrinterExists()
        {
            var printer = new Printer
            {
                Id = 1,
                ModelName = "P1S"
            };

            printerServiceMock
                .Setup(s => s.GetPrinterByIdAsync(1, "user-1"))
                .ReturnsAsync(printer);

            var controller = CreateController();

            var result = await controller.Delete(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(printer, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldReturnNotFound_WhenDeleteFails()
        {
            printerServiceMock
                .Setup(s => s.DeletePrinterAsync(1, "user-1"))
                .ReturnsAsync(false);

            var controller = CreateController();

            var result = await controller.DeleteConfirmed(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeleteSucceeds()
        {
            printerServiceMock
                .Setup(s => s.DeletePrinterAsync(1, "user-1"))
                .ReturnsAsync(true);

            var controller = CreateController();

            var result = await controller.DeleteConfirmed(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenPrinterDoesNotExist()
        {
            printerServiceMock
                .Setup(s => s.GetPrinterDetailsAsync(1, "user-1"))
                .ReturnsAsync((PrinterViewModel?)null);

            var controller = CreateController();

            var result = await controller.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenPrinterExists()
        {
            var model = new PrinterViewModel
            {
                Id = 1,
                ModelName = "P1S"
            };

            printerServiceMock
                .Setup(s => s.GetPrinterDetailsAsync(1, "user-1"))
                .ReturnsAsync(model);

            var controller = CreateController();

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }
    }
}