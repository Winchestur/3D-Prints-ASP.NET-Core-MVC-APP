using _3D_Prints_APP.Data.Repositories.Contracts;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _3D_Prints_APP.Data.Repositories
{
    public class FilamentOptionRepository : IFilamentOptionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public FilamentOptionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<FilamentOption>> GetAllAsync()
        {
            return await dbContext.FilamentOptions.ToListAsync();
        }

        public async Task<FilamentOption?> GetByIdAsync(int id)
        {
            return await dbContext.FilamentOptions.FindAsync(id);
        }

        public async Task AddAsync(FilamentOption option)
        {
            dbContext.FilamentOptions.Add(option);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(FilamentOption option)
        {
            dbContext.FilamentOptions.Remove(option);
            await Task.CompletedTask;
        }
    }
}