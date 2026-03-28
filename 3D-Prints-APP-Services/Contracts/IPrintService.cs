using _3DPrintsAPP.ViewModels;

namespace _3D_Prints_APP_Services.Contracts
{
    public interface IPrintService
    {
        Task<ICollection<PrintViewModel>> GetAllPrintsAsync();
        Task<PrintViewModel?> GetPrintDetailsAsync(int id);
        Task<PrintCreateEditViewModel> GetCreateViewModelAsync();
        Task CreatePrintAsync(PrintCreateEditViewModel model);
        Task<PrintCreateEditViewModel?> GetEditViewModelAsync(int id);
        Task EditPrintAsync(int id, PrintCreateEditViewModel model);
        Task<PrintViewModel?> GetDeleteViewModelAsync(int id);
        Task DeletePrintAsync(int id);
    }
}
