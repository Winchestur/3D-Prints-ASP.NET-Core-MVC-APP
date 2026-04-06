using _3DPrintsAPP.ViewModels;

namespace _3D_Prints_APP_Services.Contracts
{
    public interface IPrintService
    {
        Task<ICollection<PrintViewModel>> GetAllPrintsAsync(string userId);
        Task<PrintViewModel?> GetPrintDetailsAsync(int id, string userId);

        Task<PrintCreateEditViewModel?> GetEditViewModelAsync(int id, string userId);
        Task<PrintViewModel?> GetDeleteViewModelAsync(int id, string userId);
        Task<PrintCreateEditViewModel> GetCreateViewModelAsync();
        Task CreatePrintAsync(PrintCreateEditViewModel model, string userId);
        Task EditPrintAsync(int id, PrintCreateEditViewModel model, string userId);
        Task DeletePrintAsync(int id, string userId);

        Task PublishToWorldAsync(int printId, string userId);
        Task<ICollection<PrintViewModel>> GetWorldPrintsAsync(string userId);
        Task AddToCollectionAsync(int printId, string userId);
        Task RemoveFromCollectionAsync(int printId, string userId);
        Task<ICollection<PrintViewModel>> GetMyCollectionAsync(string userId);

        Task<PrintCreateEditViewModel> RebuildCreateEditViewModelAsync(PrintCreateEditViewModel model);
        Task<ICollection<PrintViewModel>> GetLatestPublicPrintsAsync(int count);

        Task MakePrivateAsync(int printId, string userId);

        Task RatePrintAsync(int printId, string userId, int value);
        //Task<PrintViewModel?> GetWorldPrintDetailsAsync(int id, string userId);
    }
}