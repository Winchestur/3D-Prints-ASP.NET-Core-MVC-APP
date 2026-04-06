using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;

namespace _3D_Prints_APP_Services
{
    public class PrintService : IPrintService
    {
        private readonly IPrintRepository printRepository;

        public PrintService(IPrintRepository printRepository)
        {
            this.printRepository = printRepository;
        }

        public async Task<ICollection<PrintViewModel>> GetAllPrintsAsync(string userId)
        {
            var prints = await printRepository.GetAllByUserIdAsync(userId);

            return prints.Select(p => new PrintViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description!,
                PrintTime = p.PrintTime,
                UploadPhoto = p.UploadPhoto!,
                UploadedTime = p.UploadedTime,
                IsPublic = p.IsPublic,
                OwnerName = p.User?.UserName,
                OwnerId = p.UserId
            }).ToList();
        }

        public async Task<PrintViewModel?> GetPrintDetailsAsync(int id, string userId)
        {
            var print = await printRepository.GetByIdWithUserAsync(id);

            if (print == null)
            {
                return null;
            }

            if (print.UserId != userId && !print.IsPublic)
            {
                throw new UnauthorizedAccessException();
            }

            return new PrintViewModel
            {
                Id = print.Id,
                Title = print.Title,
                Description = print.Description!,
                PrintTime = print.PrintTime,
                UploadPhoto = print.UploadPhoto!,
                UploadedTime = print.UploadedTime,
                IsPublic = print.IsPublic,
                OwnerName = print.User?.UserName,
                OwnerId = print.UserId
            };
        }

        public Task<PrintCreateEditViewModel> GetCreateViewModelAsync()
        {
            return Task.FromResult(new PrintCreateEditViewModel
            {
                PrintTime = new TimeOnly(1, 0)
            });
        }

        public async Task CreatePrintAsync(PrintCreateEditViewModel model, string userId)
        {
            var print = new Print
            {
                Title = model.Title,
                Description = model.Description,
                PrintTime = model.PrintTime,
                UploadPhoto = model.UploadPhoto,
                UploadedTime = DateTime.UtcNow,
                UserId = userId,
                IsPublic = false
            };

            await printRepository.AddAsync(print);
        }
        public async Task<PrintCreateEditViewModel?> GetEditViewModelAsync(int id, string userId)
        {
            var print = await printRepository.GetByIdAsync(id);

            if (print == null)
            {
                return null;
            }

            if (print.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            return new PrintCreateEditViewModel
            {
                Title = print.Title,
                Description = print.Description!,
                PrintTime = print.PrintTime,
                UploadPhoto = print.UploadPhoto!
            };
        }

        public async Task EditPrintAsync(int id, PrintCreateEditViewModel model, string userId)
        {
            var print = await printRepository.GetByIdAsync(id);

            if (print == null)
            {
                throw new KeyNotFoundException("Print not found.");
            }

            if (print.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            print.Title = model.Title;
            print.Description = model.Description;
            print.PrintTime = model.PrintTime;
            print.UploadPhoto = model.UploadPhoto;

            await printRepository.UpdateAsync(print);
        }

        public async Task<PrintViewModel?> GetDeleteViewModelAsync(int id, string userId)
        {
            var print = await printRepository.GetByIdWithUserAsync(id);

            if (print == null)
            {
                return null;
            }

            if (print.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            return new PrintViewModel
            {
                Id = print.Id,
                Title = print.Title,
                Description = print.Description!,
                PrintTime = print.PrintTime,
                UploadPhoto = print.UploadPhoto!,
                UploadedTime = print.UploadedTime,
                IsPublic = print.IsPublic,
                OwnerName = print.User?.UserName,
                OwnerId = print.UserId
            };
        }

        public async Task DeletePrintAsync(int id, string userId)
        {
            var print = await printRepository.GetByIdAsync(id);

            if (print == null)
            {
                throw new KeyNotFoundException("Print not found.");
            }

            if (print.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            await printRepository.DeleteAsync(print);
        }

        public async Task PublishToWorldAsync(int printId, string userId)
        {
            var print = await printRepository.GetByIdAsync(printId);

            if (print == null)
            {
                throw new KeyNotFoundException("Print not found.");
            }

            if (print.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            print.IsPublic = true;
            await printRepository.UpdateAsync(print);
        }

        public async Task<ICollection<PrintViewModel>> GetWorldPrintsAsync(string userId)
        {
            var prints = await printRepository.GetAllPublicAsync();
            var userCollectionIds = await printRepository.GetUserCollectionIdsAsync(userId);

            var printIds = prints.Select(p => p.Id).ToList();
            var averageRatings = await printRepository.GetAverageRatingsAsync(printIds);
            var ratingsCount = await printRepository.GetRatingsCountAsync(printIds);
            var userRatings = await printRepository.GetUserRatingsForPrintsAsync(printIds, userId);

            return prints.Select(p => new PrintViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description!,
                PrintTime = p.PrintTime,
                UploadPhoto = p.UploadPhoto!,
                UploadedTime = p.UploadedTime,
                IsPublic = p.IsPublic,
                OwnerName = p.User?.UserName,
                OwnerId = p.UserId,
                IsInCollection = userCollectionIds.Contains(p.Id),
                AverageRating = averageRatings.ContainsKey(p.Id) ? Math.Round(averageRatings[p.Id], 1) : 0,
                RatingsCount = ratingsCount.ContainsKey(p.Id) ? ratingsCount[p.Id] : 0,
                UserRating = userRatings.ContainsKey(p.Id) ? userRatings[p.Id] : null
            }).ToList();
        }

        public async Task AddToCollectionAsync(int printId, string userId)
        {
            var print = await printRepository.GetPublicByIdAsync(printId);

            if (print == null)
            {
                throw new KeyNotFoundException("Public print not found.");
            }

            var exists = await printRepository.ExistsInCollectionAsync(printId, userId);
            if (exists)
            {
                return;
            }

            await printRepository.AddToCollectionAsync(new UserCollectionPrint
            {
                PrintId = printId,
                UserId = userId
            });
        }

        public async Task RemoveFromCollectionAsync(int printId, string userId)
        {
            var exists = await printRepository.ExistsInCollectionAsync(printId, userId);

            if (!exists)
            {
                return;
            }

            await printRepository.RemoveFromCollectionAsync(printId, userId);
        }

        public async Task<ICollection<PrintViewModel>> GetMyCollectionAsync(string userId)
        {
            var prints = await printRepository.GetCollectionByUserIdAsync(userId);

            return prints.Select(p => new PrintViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description!,
                PrintTime = p.PrintTime,
                UploadPhoto = p.UploadPhoto!,
                UploadedTime = p.UploadedTime,
                IsPublic = p.IsPublic,
                OwnerName = p.User?.UserName,
                OwnerId = p.UserId
            }).ToList();
        }

        public Task<PrintCreateEditViewModel> RebuildCreateEditViewModelAsync(PrintCreateEditViewModel model)
        {
            return Task.FromResult(model);
        }

        public async Task<ICollection<PrintViewModel>> GetLatestPublicPrintsAsync(int count)
        {
            var prints = await printRepository.GetLatestPublicPrintsAsync(count);

            return prints.Select(p => new PrintViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description!,
                PrintTime = p.PrintTime,
                UploadPhoto = p.UploadPhoto!,
                UploadedTime = p.UploadedTime,
                IsPublic = p.IsPublic,
                OwnerName = p.User?.UserName,
                OwnerId = p.UserId
            }).ToList();
        }

        public async Task MakePrivateAsync(int printId, string userId)
        {
            var print = await printRepository.GetByIdAsync(printId);

            if (print == null)
            {
                throw new KeyNotFoundException("Print not found.");
            }

            if (print.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            print.IsPublic = false;

            await printRepository.UpdateAsync(print);
        }

        public async Task RatePrintAsync(int printId, string userId, int value)
        {
            var print = await printRepository.GetPublicByIdAsync(printId);

            if (print == null)
            {
                throw new KeyNotFoundException("Public print not found.");
            }

            await printRepository.AddOrUpdateRatingAsync(printId, userId, value);
        }
    }
}