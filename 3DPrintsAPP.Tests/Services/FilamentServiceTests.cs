using Moq;
using Xunit;
using _3D_Prints_APP_Services;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Filaments;
using _3DPrintsAPP.Enums;

namespace _3DPrintsAPP.Tests.Services
{
    public class FilamentServiceTests
    {
        private readonly Mock<IFilamentRepository> filamentRepositoryMock;
        private readonly Mock<IFilamentOptionService> filamentOptionServiceMock;
        private readonly FilamentService filamentService;

        public FilamentServiceTests()
        {
            filamentRepositoryMock = new Mock<IFilamentRepository>();
            filamentOptionServiceMock = new Mock<IFilamentOptionService>();

            filamentService = new FilamentService(
                filamentRepositoryMock.Object,
                filamentOptionServiceMock.Object);
        }

        [Fact]
        public async Task GetAllFilamentsAsync_ShouldReturnMappedFilaments()
        {
            var filaments = new List<Filament>
            {
                new Filament
                {
                    Id = 1,
                    Brand = Brand.eSUN,
                    Material = Materials.PLA,
                    FilamentColor = Colors.Black,
                    UploadPhoto = "black.jpg",
                    WeightKG = 1.0,
                    Diameter = 1.75m
                },
                new Filament
                {
                    Id = 2,
                    Brand = Brand.eSUN,
                    Material = Materials.PETG,
                    FilamentColor = Colors.White,
                    UploadPhoto = "white.jpg",
                    WeightKG = 2.0,
                    Diameter = 1.75m
                }
            };

            filamentRepositoryMock
                .Setup(r => r.GetAllAsync("user-1"))
                .ReturnsAsync(filaments);

            var result = (await filamentService.GetAllFilamentsAsync("user-1")).ToList();

            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal(Brand.eSUN, result[0].Brand);
            Assert.Equal(Materials.PLA, result[0].Material);
            Assert.Equal(Colors.Black, result[0].FilamentColor);
            Assert.Equal("black.jpg", result[0].UploadPhoto);
            Assert.Equal(1.0, result[0].WeightKg);
            Assert.Equal(1.75m, result[0].Diameter);

            Assert.Equal(2, result[1].Id);
            Assert.Equal(Brand.eSUN, result[1].Brand);
        }

        [Fact]
        public async Task GetFilamentDetailsAsync_ShouldReturnNull_WhenFilamentNotFound()
        {
            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(10, "user-1"))
                .ReturnsAsync((Filament?)null);

            var result = await filamentService.GetFilamentDetailsAsync(10, "user-1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetFilamentDetailsAsync_ShouldReturnMappedFilament_WhenFound()
        {
            var filament = new Filament
            {
                Id = 3,
                Brand = Brand.eSUN,
                Material = Materials.ASA,
                FilamentColor = Colors.Gray,
                UploadPhoto = "gray.jpg",
                WeightKG = 1.0,
                Diameter = 1.75m
            };

            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(3, "user-1"))
                .ReturnsAsync(filament);

            var result = await filamentService.GetFilamentDetailsAsync(3, "user-1");

            Assert.NotNull(result);
            Assert.Equal(3, result!.Id);
            Assert.Equal(Brand.eSUN, result.Brand);
            Assert.Equal(Materials.ASA, result.Material);
            Assert.Equal(Colors.Gray, result.FilamentColor);
            Assert.Equal("gray.jpg", result.UploadPhoto);
            Assert.Equal(1.0, result.WeightKg);
            Assert.Equal(1.75m, result.Diameter);
        }

        [Fact]
        public async Task GetCreateViewModelAsync_ShouldReturnModelWithFilamentOptions()
        {
            var options = new List<FilamentOption>
            {
                new FilamentOption
                {
                    Id = 1,
                    Brand = Brand.BambuLab,
                    Material = Materials.PLA,
                    FilamentColor = Colors.Black
                },
                new FilamentOption
                {
                    Id = 2,
                    Brand = Brand.eSUN,
                    Material = Materials.PETG,
                    FilamentColor =  Colors.White
                }
            };

            filamentOptionServiceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(options);

            var result = await filamentService.GetCreateViewModelAsync();

            Assert.NotNull(result);
            Assert.NotNull(result.FilamentOptions);

            var list = result.FilamentOptions.ToList();
            Assert.Equal(2, list.Count);

            Assert.Equal("1", list[0].Value);
            Assert.Equal("BambuLab - PLA - Black", list[0].Text);

            Assert.Equal("2", list[1].Value);
            Assert.Equal("eSUN - PETG - White", list[1].Text);
        }

        [Fact]
        public async Task CreateFilamentAsync_ShouldCreateFilament_WhenOptionExists()
        {
            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 5
            };

            var option = new FilamentOption
            {
                Id = 5,
                Brand = Brand.eSUN,
                Material = Materials.TPU,
                FilamentColor = Colors.Black,
                UploadPhoto = "black.jpg",
                WeightKG = 1.0,
                Diameter = 1.75m
            };

            Filament? createdFilament = null;

            filamentOptionServiceMock
                .Setup(s => s.GetByIdAsync(5))
                .ReturnsAsync(option);

            filamentRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Filament>()))
                .Callback<Filament>(f => createdFilament = f)
                .Returns(Task.CompletedTask);

            await filamentService.CreateFilamentAsync(model, "user-1");

            Assert.NotNull(createdFilament);
            Assert.Equal(Brand.eSUN, createdFilament!.Brand);
            Assert.Equal(Materials.TPU, createdFilament.Material);
            Assert.Equal(Colors.Black, createdFilament.FilamentColor);
            Assert.Equal("black.jpg", createdFilament.UploadPhoto);
            Assert.Equal(1.0, createdFilament.WeightKG);
            Assert.Equal(1.75m, createdFilament.Diameter);
            Assert.Equal("user-1", createdFilament.UserId);
            Assert.Equal(5, createdFilament.FilamentOptionId);

            filamentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Filament>()), Times.Once);
        }

        [Fact]
        public async Task CreateFilamentAsync_ShouldThrow_WhenOptionNotFound()
        {
            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 99
            };

            filamentOptionServiceMock
                .Setup(s => s.GetByIdAsync(99))
                .ReturnsAsync((FilamentOption?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                filamentService.CreateFilamentAsync(model, "user-1"));

            filamentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Filament>()), Times.Never);
        }

        [Fact]
        public async Task GetEditViewModelAsync_ShouldReturnNull_WhenFilamentNotFound()
        {
            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(20, "user-1"))
                .ReturnsAsync((Filament?)null);

            var result = await filamentService.GetEditViewModelAsync(20, "user-1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetEditViewModelAsync_ShouldReturnModelWithSelectedOption_WhenFilamentExists()
        {
            var filament = new Filament
            {
                Id = 21,
                FilamentOptionId = 2,
                UserId = "user-1"
            };

            var options = new List<FilamentOption>
            {
                new FilamentOption
                {
                    Id = 1,
                    Brand = Brand.eSUN,
                    Material = Materials.PLA,
                    FilamentColor = Colors.Black
                },
                new FilamentOption
                {
                    Id = 2,
                    Brand = Brand.eSUN,
                    Material = Materials.PETG,
                    FilamentColor = Colors.White
                }
            };

            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(21, "user-1"))
                .ReturnsAsync(filament);

            filamentOptionServiceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(options);

            var result = await filamentService.GetEditViewModelAsync(21, "user-1");

            Assert.NotNull(result);
            Assert.Equal(2, result!.FilamentOptionId);

            var list = result.FilamentOptions.ToList();
            Assert.Equal(2, list.Count);
            Assert.Equal("eSUN - PLA - Black", list[0].Text);
            Assert.Equal("eSUN - PETG - White", list[1].Text);
        }

        [Fact]
        public async Task EditFilamentAsync_ShouldUpdateFilament_WhenFilamentAndOptionExist()
        {
            var filament = new Filament
            {
                Id = 30,
                Brand = Brand.Prusament,
                Material = Materials.PVA,
                FilamentColor = Colors.Purple,
                UploadPhoto = "prusament.jpg",
                WeightKG = 0.5,
                Diameter = 2.85m,
                FilamentOptionId = 1,
                UserId = "user-1"
            };

            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 2
            };

            var option = new FilamentOption
            {
                Id = 2,
                Brand = Brand.Hatchbox,
                Material = Materials.PETG,
                FilamentColor = Colors.Blue,
                UploadPhoto = "blue.jpg",
                WeightKG = 1.0,
                Diameter = 1.75m
            };

            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(30, "user-1"))
                .ReturnsAsync(filament);

            filamentOptionServiceMock
                .Setup(s => s.GetByIdAsync(2))
                .ReturnsAsync(option);

            await filamentService.EditFilamentAsync(30, model, "user-1");

            Assert.Equal(Brand.Hatchbox, filament.Brand);
            Assert.Equal(Materials.PETG, filament.Material);
            Assert.Equal(Colors.Blue, filament.FilamentColor);
            Assert.Equal("blue.jpg", filament.UploadPhoto);
            Assert.Equal(1.0, filament.WeightKG);
            Assert.Equal(1.75m, filament.Diameter);
            Assert.Equal(2, filament.FilamentOptionId);

            filamentRepositoryMock.Verify(r => r.UpdateAsync(filament), Times.Once);
        }

        [Fact]
        public async Task EditFilamentAsync_ShouldThrow_WhenFilamentNotFound()
        {
            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 2
            };

            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(31, "user-1"))
                .ReturnsAsync((Filament?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                filamentService.EditFilamentAsync(31, model, "user-1"));

            filamentRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Filament>()), Times.Never);
        }

        [Fact]
        public async Task EditFilamentAsync_ShouldThrow_WhenOptionNotFound()
        {
            var filament = new Filament
            {
                Id = 32,
                FilamentOptionId = 1,
                UserId = "user-1"
            };

            var model = new FilamentCreateEditViewModel
            {
                FilamentOptionId = 999
            };

            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(32, "user-1"))
                .ReturnsAsync(filament);

            filamentOptionServiceMock
                .Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync((FilamentOption?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                filamentService.EditFilamentAsync(32, model, "user-1"));

            filamentRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Filament>()), Times.Never);
        }

        [Fact]
        public async Task DeleteFilamentAsync_ShouldDelete_WhenFilamentExists()
        {
            var filament = new Filament
            {
                Id = 40,
                UserId = "user-1"
            };

            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(40, "user-1"))
                .ReturnsAsync(filament);

            await filamentService.DeleteFilamentAsync(40, "user-1");

            filamentRepositoryMock.Verify(r => r.DeleteAsync(filament), Times.Once);
        }

        [Fact]
        public async Task DeleteFilamentAsync_ShouldThrow_WhenFilamentNotFound()
        {
            filamentRepositoryMock
                .Setup(r => r.GetByIdAsync(41, "user-1"))
                .ReturnsAsync((Filament?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                filamentService.DeleteFilamentAsync(41, "user-1"));

            filamentRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Filament>()), Times.Never);
        }
    }
}