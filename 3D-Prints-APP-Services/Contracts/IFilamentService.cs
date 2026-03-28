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
        Task<ICollection<FilamentViewModel>> GetAllFilamentsAsync();
        Task<FilamentViewModel?> GetFilamentDetailsAsync(int id);
        Task<FilamentCreateEditViewModel> GetCreateViewModelAsync();
        Task CreateFilamentAsync(FilamentCreateEditViewModel model);
        Task<FilamentCreateEditViewModel?> GetEditViewModelAsync(int id);
        Task EditFilamentAsync(int id, FilamentCreateEditViewModel model);
        Task DeleteFilamentAsync(int id);
    }
}
