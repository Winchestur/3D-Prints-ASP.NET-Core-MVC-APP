using System.Security.Claims;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Controllers;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;

namespace _3DPrintsAPP.Tests.Controllers
{
    public class PrintsControllerTests
    {
        private readonly Mock<IPrintService> printServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;

        public PrintsControllerTests()
        {
            printServiceMock = new Mock<IPrintService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null!, null!, null!, null!, null!, null!, null!, null!);
        }

        private PrintsController CreateController(string? userId = "user-1")
        {
            var controller = new PrintsController(printServiceMock.Object, userManagerMock.Object);

            var principal = userId == null
                ? new ClaimsPrincipal(new ClaimsIdentity())
                : new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }, "TestAuth"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };

            userManagerMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            return controller;
        }

        [Fact]
        public async Task Index_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.Index();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithPrints_WhenUserExists()
        {
            var prints = new List<PrintViewModel>
            {
                new PrintViewModel { Id = 1, Title = "Print 1" },
                new PrintViewModel { Id = 2, Title = "Print 2" }
            };

            printServiceMock
                .Setup(s => s.GetAllPrintsAsync("user-1"))
                .ReturnsAsync(prints);

            var controller = CreateController("user-1");

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ICollection<PrintViewModel>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.Details(1, null);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenServiceReturnsNull()
        {
            printServiceMock
                .Setup(s => s.GetPrintDetailsAsync(1, "user-1"))
                .ReturnsAsync((PrintViewModel?)null);

            var controller = CreateController("user-1");

            var result = await controller.Details(1, null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnForbid_WhenServiceThrowsUnauthorizedAccessException()
        {
            printServiceMock
                .Setup(s => s.GetPrintDetailsAsync(1, "user-1"))
                .ThrowsAsync(new UnauthorizedAccessException());

            var controller = CreateController("user-1");

            var result = await controller.Details(1, null);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenPrintExists()
        {
            var viewModel = new PrintViewModel
            {
                Id = 1,
                Title = "My Print"
            };

            printServiceMock
                .Setup(s => s.GetPrintDetailsAsync(1, "user-1"))
                .ReturnsAsync(viewModel);

            var controller = CreateController("user-1");

            var result = await controller.Details(1, "/Prints");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PrintViewModel>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("/Prints", model.ReturnUrl);
        }

        [Fact]
        public async Task CreateGet_ShouldReturnViewWithModel()
        {
            var model = new PrintCreateEditViewModel
            {
                Title = "Test",
                PrintTime = new TimeOnly(1, 0)
            };

            printServiceMock
                .Setup(s => s.GetCreateViewModelAsync())
                .ReturnsAsync(model);

            var controller = CreateController("user-1");

            var result = await controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.Create(new PrintCreateEditViewModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreatePost_ShouldReturnView_WhenModelStateIsInvalid()
        {
            var inputModel = new PrintCreateEditViewModel();
            var rebuiltModel = new PrintCreateEditViewModel
            {
                Title = "Rebuilt"
            };

            printServiceMock
                .Setup(s => s.RebuildCreateEditViewModelAsync(inputModel))
                .ReturnsAsync(rebuiltModel);

            var controller = CreateController("user-1");
            controller.ModelState.AddModelError("Title", "Required");

            var result = await controller.Create(inputModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(rebuiltModel, viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            var model = new PrintCreateEditViewModel
            {
                Title = "New Print"
            };

            var controller = CreateController("user-1");

            var result = await controller.Create(model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            printServiceMock.Verify(s => s.CreatePrintAsync(model, "user-1"), Times.Once);
        }

        [Fact]
        public async Task EditGet_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.Edit(5);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task EditGet_ShouldReturnNotFound_WhenServiceReturnsNull()
        {
            printServiceMock
                .Setup(s => s.GetEditViewModelAsync(5, "user-1"))
                .ReturnsAsync((PrintCreateEditViewModel?)null);

            var controller = CreateController("user-1");

            var result = await controller.Edit(5);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ShouldReturnView_WhenModelExists()
        {
            var model = new PrintCreateEditViewModel
            {
                Title = "Edit Me"
            };

            printServiceMock
                .Setup(s => s.GetEditViewModelAsync(5, "user-1"))
                .ReturnsAsync(model);

            var controller = CreateController("user-1");

            var result = await controller.Edit(5);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.Edit(1, new PrintCreateEditViewModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task EditPost_ShouldReturnView_WhenModelStateIsInvalid()
        {
            var model = new PrintCreateEditViewModel();
            var rebuiltModel = new PrintCreateEditViewModel
            {
                Title = "Rebuilt"
            };

            printServiceMock
                .Setup(s => s.RebuildCreateEditViewModelAsync(model))
                .ReturnsAsync(rebuiltModel);

            var controller = CreateController("user-1");
            controller.ModelState.AddModelError("Title", "Required");

            var result = await controller.Edit(1, model);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(rebuiltModel, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFound_WhenServiceThrowsKeyNotFoundException()
        {
            var model = new PrintCreateEditViewModel();

            printServiceMock
                .Setup(s => s.EditPrintAsync(1, model, "user-1"))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = CreateController("user-1");

            var result = await controller.Edit(1, model);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ShouldReturnForbid_WhenServiceThrowsUnauthorizedAccessException()
        {
            var model = new PrintCreateEditViewModel();

            printServiceMock
                .Setup(s => s.EditPrintAsync(1, model, "user-1"))
                .ThrowsAsync(new UnauthorizedAccessException());

            var controller = CreateController("user-1");

            var result = await controller.Edit(1, model);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task EditPost_ShouldRedirectToIndex_WhenSuccessful()
        {
            var model = new PrintCreateEditViewModel
            {
                Title = "Edited"
            };

            var controller = CreateController("user-1");

            var result = await controller.Edit(1, model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            printServiceMock.Verify(s => s.EditPrintAsync(1, model, "user-1"), Times.Once);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.Delete(1);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnNotFound_WhenServiceReturnsNull()
        {
            printServiceMock
                .Setup(s => s.GetDeleteViewModelAsync(1, "user-1"))
                .ReturnsAsync((PrintViewModel?)null);

            var controller = CreateController("user-1");

            var result = await controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnView_WhenModelExists()
        {
            var model = new PrintViewModel
            {
                Id = 1,
                Title = "Delete Me"
            };

            printServiceMock
                .Setup(s => s.GetDeleteViewModelAsync(1, "user-1"))
                .ReturnsAsync(model);

            var controller = CreateController("user-1");

            var result = await controller.Delete(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.DeleteConfirmed(1);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldReturnNotFound_WhenServiceThrowsKeyNotFoundException()
        {
            printServiceMock
                .Setup(s => s.DeletePrintAsync(1, "user-1"))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = CreateController("user-1");

            var result = await controller.DeleteConfirmed(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldReturnForbid_WhenServiceThrowsUnauthorizedAccessException()
        {
            printServiceMock
                .Setup(s => s.DeletePrintAsync(1, "user-1"))
                .ThrowsAsync(new UnauthorizedAccessException());

            var controller = CreateController("user-1");

            var result = await controller.DeleteConfirmed(1);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenSuccessful()
        {
            var controller = CreateController("user-1");

            var result = await controller.DeleteConfirmed(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            printServiceMock.Verify(s => s.DeletePrintAsync(1, "user-1"), Times.Once);
        }

        [Fact]
        public async Task PublishToWorld_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.PublishToWorld(1);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task PublishToWorld_ShouldReturnNotFound_WhenServiceThrowsKeyNotFoundException()
        {
            printServiceMock
                .Setup(s => s.PublishToWorldAsync(1, "user-1"))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = CreateController("user-1");

            var result = await controller.PublishToWorld(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PublishToWorld_ShouldReturnForbid_WhenServiceThrowsUnauthorizedAccessException()
        {
            printServiceMock
                .Setup(s => s.PublishToWorldAsync(1, "user-1"))
                .ThrowsAsync(new UnauthorizedAccessException());

            var controller = CreateController("user-1");

            var result = await controller.PublishToWorld(1);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task PublishToWorld_ShouldRedirectToIndex_WhenSuccessful()
        {
            var controller = CreateController("user-1");

            var result = await controller.PublishToWorld(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            printServiceMock.Verify(s => s.PublishToWorldAsync(1, "user-1"), Times.Once);
        }

        [Fact]
        public async Task MakePrivate_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            var controller = CreateController(null);

            var result = await controller.MakePrivate(1);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task MakePrivate_ShouldReturnNotFound_WhenServiceThrowsKeyNotFoundException()
        {
            printServiceMock
                .Setup(s => s.MakePrivateAsync(1, "user-1"))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = CreateController("user-1");

            var result = await controller.MakePrivate(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task MakePrivate_ShouldReturnForbid_WhenServiceThrowsUnauthorizedAccessException()
        {
            printServiceMock
                .Setup(s => s.MakePrivateAsync(1, "user-1"))
                .ThrowsAsync(new UnauthorizedAccessException());

            var controller = CreateController("user-1");

            var result = await controller.MakePrivate(1);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task MakePrivate_ShouldRedirectToIndex_WhenSuccessful()
        {
            var controller = CreateController("user-1");

            var result = await controller.MakePrivate(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            printServiceMock.Verify(s => s.MakePrivateAsync(1, "user-1"), Times.Once);
        }
    }
}