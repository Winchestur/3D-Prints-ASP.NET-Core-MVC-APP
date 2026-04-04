using _3DPrintsAPP.ViewModels.Filaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP_Services.Contracts
{
    public interface IFilamentService
    {
        Task<ICollection<FilamentViewModel>> GetAllFilamentsAsync(string userId);
        Task<FilamentViewModel?> GetFilamentDetailsAsync(int id, string userId);
        Task<FilamentCreateEditViewModel> GetCreateViewModelAsync();
        Task CreateFilamentAsync(FilamentCreateEditViewModel model, string userId);
        Task<FilamentCreateEditViewModel?> GetEditViewModelAsync(int id, string userId);
        Task EditFilamentAsync(int id, FilamentCreateEditViewModel model, string userId);
        Task DeleteFilamentAsync(int id, string userId);
    }
}
