using _3DPrintsAPP.Data.Models;

namespace _3D_Prints_APP_Services.Contracts
{
    public interface IFilamentOptionService
    {
        Task<List<FilamentOption>> GetAllAsync();
        Task<FilamentOption?> GetByIdAsync(int id);
        Task CreateAsync(FilamentOption option);
        Task<bool> UpdateAsync(int id, FilamentOption model);
        Task<bool> DeleteAsync(int id);
    }
}