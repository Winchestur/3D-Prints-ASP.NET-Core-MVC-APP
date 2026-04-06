using System.Security.Claims;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Controllers;
using _3DPrintsAPP.ViewModels.Filaments;

namespace _3DPrintsAPP.Tests.Controllers
{
    public class FilamentsControllerTests
    {
        private readonly Mock<IFilamentService> filamentServiceMock;

        public FilamentsControllerTests()
        {
            filamentServiceMock = new Mock<IFilamentService>();
        }

        private FilamentsController CreateController(string userId = "user-1")
        {
            var controller = new FilamentsController(filamentServiceMock.Object);

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
        public async Task Index_ShouldReturnViewWithFilaments()
        {
            var filaments = new List<FilamentViewModel>
            {
                new FilamentViewModel { Id = 1 },
                new FilamentViewModel { Id = 2 }
            };

            filamentServiceMock
                .Setup(s => s.GetAllFilamentsAsync("user-1"))
                .ReturnsAsync(filaments);

            var controller = CreateController();

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<FilamentViewModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenFilamentDoesNotExist()
        {
            filamentServiceMock
                .Setup(s => s.GetFilamentDetailsAsync(1, "user-1"))
                .ReturnsAsync((FilamentViewModel?)null);

            var controller = CreateController();

            var result = await controller.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenFilamentExists()
        {
            var model = new FilamentViewModel
            {
                Id = 1
            };

            filamentServiceMock
                .Setup(s => s.GetFilamentDetailsAsync(1, "user-1"))
                .ReturnsAsync(model);

            var controller = CreateController();

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task CreateGet_ShouldReturnViewWithModel()
        {
            var model = new FilamentCreateEditViewModel
            {
                FilamentOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Option 1" }
                }
            };

            filamentServiceMock
                .Setup(s => s.GetCreateViewModelAsync())
                .ReturnsAsync(model);

            var controller = CreateController();

            var result = await controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_ShouldReturnViewWithFreshModel_WhenModelStateIsInvalid()
        {
            var freshModel = new FilamentCreateEditViewModel
            {
                FilamentOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Option 1" }
                }
            };

            filamentServiceMock
                .Setup(s => s.GetCreateViewModelAsync())
                .ReturnsAsync(freshModel);

            var controller = CreateController();
            controller.ModelState.AddModelError("FilamentOptionId", "Required");

            var result = await controller.Create(new FilamentCreateEditViewModel());

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(freshModel, viewResult.Model);

            filamentServiceMock.Verify(s => s.CreateFilamentAsync(It.IsAny<FilamentCreateEditViewModel>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreatePost_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 1
            };

            var controller = CreateController();

            var result = await controller.Create(model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            filamentServiceMock.Verify(s => s.CreateFilamentAsync(model, "user-1"), Times.Once);
        }

        [Fact]
        public async Task EditGet_ShouldReturnNotFound_WhenFilamentDoesNotExist()
        {
            filamentServiceMock
                .Setup(s => s.GetEditViewModelAsync(1, "user-1"))
                .ReturnsAsync((FilamentCreateEditViewModel?)null);

            var controller = CreateController();

            var result = await controller.Edit(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ShouldReturnView_WhenFilamentExists()
        {
            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 1
            };

            filamentServiceMock
                .Setup(s => s.GetEditViewModelAsync(1, "user-1"))
                .ReturnsAsync(model);

            var controller = CreateController();

            var result = await controller.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ShouldReturnViewWithFreshModel_WhenModelStateIsInvalid()
        {
            var invalidModel = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 1
            };

            var rebuiltModel = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 2,
                FilamentOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "2", Text = "Option 2" }
                }
            };

            filamentServiceMock
                .Setup(s => s.GetEditViewModelAsync(1, "user-1"))
                .ReturnsAsync(rebuiltModel);

            var controller = CreateController();
            controller.ModelState.AddModelError("FilamentOptionId", "Required");

            var result = await controller.Edit(1, invalidModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(rebuiltModel, viewResult.Model);

            filamentServiceMock.Verify(s => s.EditFilamentAsync(It.IsAny<int>(), It.IsAny<FilamentCreateEditViewModel>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 1
            };

            var controller = CreateController();

            var result = await controller.Edit(1, model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            filamentServiceMock.Verify(s => s.EditFilamentAsync(1, model, "user-1"), Times.Once);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnNotFound_WhenFilamentDoesNotExist()
        {
            filamentServiceMock
                .Setup(s => s.GetFilamentDetailsAsync(1, "user-1"))
                .ReturnsAsync((FilamentViewModel?)null);

            var controller = CreateController();

            var result = await controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnView_WhenFilamentExists()
        {
            var model = new FilamentViewModel
            {
                Id = 1
            };

            filamentServiceMock
                .Setup(s => s.GetFilamentDetailsAsync(1, "user-1"))
                .ReturnsAsync(model);

            var controller = CreateController();

            var result = await controller.Delete(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldReturnNotFound_WhenServiceThrowsKeyNotFoundException()
        {
            filamentServiceMock
                .Setup(s => s.DeleteFilamentAsync(1, "user-1"))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = CreateController();

            var result = await controller.DeleteConfirmed(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeleteSucceeds()
        {
            var controller = CreateController();

            var result = await controller.DeleteConfirmed(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            filamentServiceMock.Verify(s => s.DeleteFilamentAsync(1, "user-1"), Times.Once);
        }
    }
}