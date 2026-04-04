using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _3D_Prints_APP_Services
{
    public class FilamentRepository : IFilamentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public FilamentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Filament>> GetAllAsync(string userId)
        {
            return await dbContext.Filaments
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<Filament?> GetByIdAsync(int id, string userId)
        {
            return await dbContext.Filaments
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
        }

        public async Task AddAsync(Filament filament)
        {
            dbContext.Filaments.Add(filament);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Filament filament)
        {
            dbContext.Filaments.Update(filament);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Filament filament)
        {
            dbContext.Filaments.Remove(filament);
            await dbContext.SaveChangesAsync();
        }
    }
}