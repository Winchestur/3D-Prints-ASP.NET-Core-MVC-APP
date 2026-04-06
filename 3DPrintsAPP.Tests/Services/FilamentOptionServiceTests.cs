using Moq;
using Xunit;
using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.Enums;

namespace _3DPrintsAPP.Tests.Services
{
    public class FilamentOptionServiceTests
    {
        private readonly Mock<IFilamentOptionRepository> filamentOptionRepositoryMock;
        private readonly FilamentOptionService filamentOptionService;

        public FilamentOptionServiceTests()
        {
            filamentOptionRepositoryMock = new Mock<IFilamentOptionRepository>();
            filamentOptionService = new FilamentOptionService(filamentOptionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllFilamentOptions()
        {
            var options = new List<FilamentOption>
            {
                new FilamentOption
                {
                    Id = 1,
                    Brand = Brand.eSUN,
                    Material = Materials.PLA,
                    FilamentColor = Colors.Black,
                    UploadPhoto = "sunlu-black.jpg",
                    WeightKG = 1.0,
                    Diameter = 1.75m
                },
                new FilamentOption
                {
                    Id = 2,
                    Brand = Brand.eSUN,
                    Material = Materials.PETG,
                    FilamentColor = Colors.White,
                    UploadPhoto = "esun-white.jpg",
                    WeightKG = 1.0,
                    Diameter = 1.75m
                }
            };

            filamentOptionRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(options);

            var result = (await filamentOptionService.GetAllAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(Brand.eSUN, result[0].Brand);
            Assert.Equal(Brand.eSUN, result[1].Brand);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOption_WhenExists()
        {
            var option = new FilamentOption
            {
                Id = 5,
                Brand = Brand.Hatchbox,
                Material = Materials.PLA,
                FilamentColor = Colors.Gray,
                UploadPhoto = "gray.jpg",
                WeightKG = 1.0,
                Diameter = 1.75m
            };

            filamentOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(option);

            var result = await filamentOptionService.GetByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal(5, result!.Id);
            Assert.Equal(Brand.Hatchbox, result.Brand);
            Assert.Equal(Materials.PLA, result.Material);
            Assert.Equal(Colors.Gray, result.FilamentColor);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            filamentOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((FilamentOption?)null);

            var result = await filamentOptionService.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAddAsync_WithGivenOption()
        {
            var option = new FilamentOption
            {
                Id = 10,
                Brand = Brand.ColorFabb,
                Material = Materials.PETG,
                FilamentColor = Colors.Blue,
                UploadPhoto = "blue.jpg",
                WeightKG = 2.0,
                Diameter = 1.75m
            };

            await filamentOptionService.CreateAsync(option);

            filamentOptionRepositoryMock.Verify(r => r.AddAsync(option), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenOptionNotFound()
        {
            var model = new FilamentOption
            {
                Brand = Brand.eSUN,
                Material = Materials.PLA,
                FilamentColor = Colors.Black,
                UploadPhoto = "black.jpg",
                WeightKG = 1.0,
                Diameter = 1.75m
            };

            filamentOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync((FilamentOption?)null);

            var result = await filamentOptionService.UpdateAsync(20, model);

            Assert.False(result);
            filamentOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOptionAndSave_WhenOptionExists()
        {
            var existingOption = new FilamentOption
            {
                Id = 21,
                Brand = Brand.eSUN,
                Material = Materials.PLA,
                FilamentColor = Colors.White,
                UploadPhoto = "old.jpg",
                WeightKG = 1.0,
                Diameter = 1.75m
            };

            var model = new FilamentOption
            {
                Brand = Brand.Hatchbox,
                Material = Materials.PETG,
                FilamentColor = Colors.Blue,
                UploadPhoto = "new.jpg",
                WeightKG = 2.0,
                Diameter = 2.85m
            };

            filamentOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(21))
                .ReturnsAsync(existingOption);

            var result = await filamentOptionService.UpdateAsync(21, model);

            Assert.True(result);
            Assert.Equal(model.Brand, existingOption.Brand);
            Assert.Equal(model.Material, existingOption.Material);
            Assert.Equal(model.FilamentColor, existingOption.FilamentColor);
            Assert.Equal(model.UploadPhoto, existingOption.UploadPhoto);
            Assert.Equal(model.WeightKG, existingOption.WeightKG);
            Assert.Equal(model.Diameter, existingOption.Diameter);

            filamentOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenOptionNotFound()
        {
            filamentOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(30))
                .ReturnsAsync((FilamentOption?)null);

            var result = await filamentOptionService.DeleteAsync(30);

            Assert.False(result);
            filamentOptionRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<FilamentOption>()), Times.Never);
            filamentOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOptionAndSave_WhenOptionExists()
        {
            var option = new FilamentOption
            {
                Id = 31,
                Brand = Brand.eSUN,
                Material = Materials.PLA,
                FilamentColor = Colors.Black
            };

            filamentOptionRepositoryMock
                .Setup(r => r.GetByIdAsync(31))
                .ReturnsAsync(option);

            var result = await filamentOptionService.DeleteAsync(31);

            Assert.True(result);
            filamentOptionRepositoryMock.Verify(r => r.DeleteAsync(option), Times.Once);
            filamentOptionRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}