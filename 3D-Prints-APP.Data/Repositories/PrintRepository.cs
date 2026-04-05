using _3D_Prints_APP.Data.Repositories.Contracts;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _3D_Prints_APP.Data.Repositories
{
    public class PrintRepository : IPrintRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PrintRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Print>> GetAllByUserIdAsync(string userId)
        {
            return await dbContext.Prints
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<ICollection<Print>> GetAllPublicAsync()
        {
            return await dbContext.Prints
                .Include(p => p.User)
                .Where(p => p.IsPublic)
                .ToListAsync();
        }

        public async Task<Print?> GetByIdAsync(int id)
        {
            return await dbContext.Prints
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Print?> GetPublicByIdAsync(int id)
        {
            return await dbContext.Prints
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsPublic);
        }

        public async Task AddAsync(Print print)
        {
            await dbContext.Prints.AddAsync(print);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Print print)
        {
            dbContext.Prints.Update(print);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Print print)
        {
            dbContext.Prints.Remove(print);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsInCollectionAsync(int printId, string userId)
        {
            return await dbContext.UserCollectionPrints
                .AnyAsync(x => x.PrintId == printId && x.UserId == userId);
        }

        public async Task AddToCollectionAsync(UserCollectionPrint entity)
        {
            await dbContext.UserCollectionPrints.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Print>> GetCollectionByUserIdAsync(string userId)
        {
            return await dbContext.UserCollectionPrints
                .Where(x => x.UserId == userId)
                .Include(x => x.Print)
                .ThenInclude(p => p.User)
                .Select(x => x.Print)
                .ToListAsync();
        }

        public async Task<ICollection<Print>> GetLatestPublicPrintsAsync(int count)
        {
            return await dbContext.Prints
                .Include(p => p.User)
                .Where(p => p.IsPublic)
                .OrderByDescending(p => p.UploadedTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Print?> GetByIdWithUserAsync(int id)
        {
            return await dbContext.Prints
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task RemoveFromCollectionAsync(int printId, string userId)
        {
            var entity = await dbContext.UserCollectionPrints
                .FirstOrDefaultAsync(x => x.PrintId == printId && x.UserId == userId);

            if (entity != null)
            {
                dbContext.UserCollectionPrints.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<HashSet<int>> GetUserCollectionIdsAsync(string userId)
        {
            return await dbContext.UserCollectionPrints
                .Where(x => x.UserId == userId)
                .Select(x => x.PrintId)
                .ToHashSetAsync();
        }
    }
}