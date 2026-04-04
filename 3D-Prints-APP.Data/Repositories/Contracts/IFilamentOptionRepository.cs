using _3DPrintsAPP.Data.Models;

namespace _3D_Prints_APP.Data.Repositories.Contracts
{
    public interface IFilamentOptionRepository
    {
        Task<List<FilamentOption>> GetAllAsync();
        Task<FilamentOption?> GetByIdAsync(int id);
        Task AddAsync(FilamentOption option);
        Task SaveChangesAsync();
        Task DeleteAsync(FilamentOption option);
    }
}