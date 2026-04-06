using Moq;
using Xunit;
using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;

namespace _3DPrintsAPP.Tests.Services
{
    public class PrintServiceTests
    {
        private readonly Mock<IPrintRepository> printRepositoryMock;
        private readonly PrintService printService;

        public PrintServiceTests()
        {
            printRepositoryMock = new Mock<IPrintRepository>();
            printService = new PrintService(printRepositoryMock.Object);
        }

        [Fact]
        public async Task GetPrintDetailsAsync_ShouldReturnPrint_WhenUserIsOwner()
        {
            var print = new Print
            {
                Id = 1,
                Title = "My Private Print",
                Description = "Desc",
                PrintTime = new TimeOnly(1, 30),
                UploadPhoto = "photo.jpg",
                UploadedTime = DateTime.UtcNow,
                UserId = "owner-id",
                IsPublic = false,
                User = new ApplicationUser { UserName = "owner-user" }
            };

            printRepositoryMock
                .Setup(r => r.GetByIdWithUserAsync(1))
                .ReturnsAsync(print);

            var result = await printService.GetPrintDetailsAsync(1, "owner-id");

            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.Equal("My Private Print", result.Title);
            Assert.Equal("owner-id", result.OwnerId);
            Assert.False(result.IsPublic);
        }

        [Fact]
        public async Task GetPrintDetailsAsync_ShouldReturnPrint_WhenPrintIsPublic()
        {
            var print = new Print
            {
                Id = 2,
                Title = "Public Print",
                Description = "Desc",
                PrintTime = new TimeOnly(2, 0),
                UploadPhoto = "photo.jpg",
                UploadedTime = DateTime.UtcNow,
                UserId = "owner-id",
                IsPublic = true,
                User = new ApplicationUser { UserName = "owner-user" }
            };

            printRepositoryMock
                .Setup(r => r.GetByIdWithUserAsync(2))
                .ReturnsAsync(print);

            var result = await printService.GetPrintDetailsAsync(2, "other-user");

            Assert.NotNull(result);
            Assert.Equal("Public Print", result!.Title);
            Assert.True(result.IsPublic);
        }

        [Fact]
        public async Task GetPrintDetailsAsync_ShouldThrow_WhenPrintIsPrivateAndUserIsNotOwner()
        {
            var print = new Print
            {
                Id = 3,
                Title = "Secret Print",
                Description = "Desc",
                PrintTime = new TimeOnly(1, 0),
                UploadPhoto = "photo.jpg",
                UploadedTime = DateTime.UtcNow,
                UserId = "owner-id",
                IsPublic = false,
                User = new ApplicationUser { UserName = "owner-user" }
            };

            printRepositoryMock
                .Setup(r => r.GetByIdWithUserAsync(3))
                .ReturnsAsync(print);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                printService.GetPrintDetailsAsync(3, "other-user"));
        }

        [Fact]
        public async Task CreatePrintAsync_ShouldCreatePrintWithCorrectUserAndPrivateStatus()
        {
            var model = new PrintCreateEditViewModel
            {
                Title = "New Print",
                Description = "Desc",
                PrintTime = new TimeOnly(3, 15),
                UploadPhoto = "photo.jpg"
            };

            Print? createdPrint = null;

            printRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Print>()))
                .Callback<Print>(p => createdPrint = p)
                .Returns(Task.CompletedTask);

            await printService.CreatePrintAsync(model, "user-123");

            Assert.NotNull(createdPrint);
            Assert.Equal("New Print", createdPrint!.Title);
            Assert.Equal("user-123", createdPrint.UserId);
            Assert.False(createdPrint.IsPublic);
        }

        [Fact]
        public async Task EditPrintAsync_ShouldUpdatePrint_WhenUserIsOwner()
        {
            var print = new Print
            {
                Id = 4,
                Title = "Old Title",
                Description = "Old Desc",
                PrintTime = new TimeOnly(1, 0),
                UploadPhoto = "old.jpg",
                UserId = "owner-id"
            };

            var model = new PrintCreateEditViewModel
            {
                Title = "New Title",
                Description = "New Desc",
                PrintTime = new TimeOnly(2, 30),
                UploadPhoto = "new.jpg"
            };

            printRepositoryMock
                .Setup(r => r.GetByIdAsync(4))
                .ReturnsAsync(print);

            await printService.EditPrintAsync(4, model, "owner-id");

            Assert.Equal("New Title", print.Title);
            Assert.Equal("New Desc", print.Description);
            Assert.Equal(new TimeOnly(2, 30), print.PrintTime);
            Assert.Equal("new.jpg", print.UploadPhoto);

            printRepositoryMock.Verify(r => r.UpdateAsync(print), Times.Once);
        }

        [Fact]
        public async Task EditPrintAsync_ShouldThrow_WhenUserIsNotOwner()
        {
            var print = new Print
            {
                Id = 5,
                Title = "Title",
                Description = "Desc",
                PrintTime = new TimeOnly(1, 0),
                UploadPhoto = "img.jpg",
                UserId = "owner-id"
            };

            var model = new PrintCreateEditViewModel
            {
                Title = "Changed",
                Description = "Changed",
                PrintTime = new TimeOnly(2, 0),
                UploadPhoto = "changed.jpg"
            };

            printRepositoryMock
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(print);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                printService.EditPrintAsync(5, model, "other-user"));
        }

        [Fact]
        public async Task DeletePrintAsync_ShouldCallDelete_WhenUserIsOwner()
        {
            var print = new Print
            {
                Id = 6,
                UserId = "owner-id"
            };

            printRepositoryMock
                .Setup(r => r.GetByIdAsync(6))
                .ReturnsAsync(print);

            await printService.DeletePrintAsync(6, "owner-id");

            printRepositoryMock.Verify(r => r.DeleteAsync(print), Times.Once);
        }

        [Fact]
        public async Task DeletePrintAsync_ShouldThrow_WhenUserIsNotOwner()
        {
            var print = new Print
            {
                Id = 7,
                UserId = "owner-id"
            };

            printRepositoryMock
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(print);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                printService.DeletePrintAsync(7, "other-user"));
        }

        [Fact]
        public async Task PublishToWorldAsync_ShouldSetIsPublicToTrue_WhenUserIsOwner()
        {
            var print = new Print
            {
                Id = 8,
                UserId = "owner-id",
                IsPublic = false
            };

            printRepositoryMock
                .Setup(r => r.GetByIdAsync(8))
                .ReturnsAsync(print);

            await printService.PublishToWorldAsync(8, "owner-id");

            Assert.True(print.IsPublic);
            printRepositoryMock.Verify(r => r.UpdateAsync(print), Times.Once);
        }

        [Fact]
        public async Task MakePrivateAsync_ShouldSetIsPublicToFalse_WhenUserIsOwner()
        {
            var print = new Print
            {
                Id = 9,
                UserId = "owner-id",
                IsPublic = true
            };

            printRepositoryMock
                .Setup(r => r.GetByIdAsync(9))
                .ReturnsAsync(print);

            await printService.MakePrivateAsync(9, "owner-id");

            Assert.False(print.IsPublic);
            printRepositoryMock.Verify(r => r.UpdateAsync(print), Times.Once);
        }

        [Fact]
        public async Task AddToCollectionAsync_ShouldAdd_WhenPrintIsPublicAndNotAlreadyAdded()
        {
            var print = new Print
            {
                Id = 10,
                IsPublic = true,
                UserId = "owner-id"
            };

            UserCollectionPrint? createdEntity = null;

            printRepositoryMock
                .Setup(r => r.GetPublicByIdAsync(10))
                .ReturnsAsync(print);

            printRepositoryMock
                .Setup(r => r.ExistsInCollectionAsync(10, "user-1"))
                .ReturnsAsync(false);

            printRepositoryMock
                .Setup(r => r.AddToCollectionAsync(It.IsAny<UserCollectionPrint>()))
                .Callback<UserCollectionPrint>(x => createdEntity = x)
                .Returns(Task.CompletedTask);

            await printService.AddToCollectionAsync(10, "user-1");

            Assert.NotNull(createdEntity);
            Assert.Equal(10, createdEntity!.PrintId);
            Assert.Equal("user-1", createdEntity.UserId);
        }

        [Fact]
        public async Task AddToCollectionAsync_ShouldNotAdd_WhenAlreadyExists()
        {
            var print = new Print
            {
                Id = 11,
                IsPublic = true,
                UserId = "owner-id"
            };

            printRepositoryMock
                .Setup(r => r.GetPublicByIdAsync(11))
                .ReturnsAsync(print);

            printRepositoryMock
                .Setup(r => r.ExistsInCollectionAsync(11, "user-1"))
                .ReturnsAsync(true);

            await printService.AddToCollectionAsync(11, "user-1");

            printRepositoryMock.Verify(r => r.AddToCollectionAsync(It.IsAny<UserCollectionPrint>()), Times.Never);
        }

        [Fact]
        public async Task RemoveFromCollectionAsync_ShouldCallRepository_WhenEntryExists()
        {
            printRepositoryMock
                .Setup(r => r.ExistsInCollectionAsync(12, "user-1"))
                .ReturnsAsync(true);

            await printService.RemoveFromCollectionAsync(12, "user-1");

            printRepositoryMock.Verify(r => r.RemoveFromCollectionAsync(12, "user-1"), Times.Once);
        }

        [Fact]
        public async Task RatePrintAsync_ShouldCallRepository_WhenPrintIsPublic()
        {
            var print = new Print
            {
                Id = 13,
                IsPublic = true,
                UserId = "owner-id"
            };

            printRepositoryMock
                .Setup(r => r.GetPublicByIdAsync(13))
                .ReturnsAsync(print);

            await printService.RatePrintAsync(13, "user-1", 5);

            printRepositoryMock.Verify(r => r.AddOrUpdateRatingAsync(13, "user-1", 5), Times.Once);
        }

        [Fact]
        public async Task GetWorldPrintsAsync_ShouldPopulateCollectionAndRatings()
        {
            var prints = new List<Print>
            {
                new Print
                {
                    Id = 14,
                    Title = "World Print",
                    Description = "Desc",
                    PrintTime = new TimeOnly(2, 0),
                    UploadPhoto = "img.jpg",
                    UploadedTime = DateTime.UtcNow,
                    IsPublic = true,
                    UserId = "owner-id",
                    User = new ApplicationUser { UserName = "owner-user" }
                }
            };

            printRepositoryMock
                .Setup(r => r.GetAllPublicAsync())
                .ReturnsAsync(prints);

            printRepositoryMock
                .Setup(r => r.GetUserCollectionIdsAsync("user-1"))
                .ReturnsAsync(new HashSet<int> { 14 });

            printRepositoryMock
                .Setup(r => r.GetAverageRatingsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new Dictionary<int, double> { { 14, 4.5 } });

            printRepositoryMock
                .Setup(r => r.GetRatingsCountAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new Dictionary<int, int> { { 14, 2 } });

            printRepositoryMock
                .Setup(r => r.GetUserRatingsForPrintsAsync(It.IsAny<IEnumerable<int>>(), "user-1"))
                .ReturnsAsync(new Dictionary<int, int> { { 14, 5 } });

            var result = await printService.GetWorldPrintsAsync("user-1");

            var item = Assert.Single(result);
            Assert.True(item.IsInCollection);
            Assert.Equal(4.5, item.AverageRating);
            Assert.Equal(2, item.RatingsCount);
            Assert.Equal(5, item.UserRating);
        }
    }
}