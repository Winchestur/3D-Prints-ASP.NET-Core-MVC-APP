using _3DPrintsAPP.Data.Models;

namespace _3D_Prints_APP.Data.Repositories.Contracts
{
    public interface IPrintRepository
    {
        Task<ICollection<Print>> GetAllByUserIdAsync(string userId);
        Task<ICollection<Print>> GetAllPublicAsync();

        Task<Print?> GetByIdAsync(int id);
        Task<Print?> GetByIdWithUserAsync(int id);
        Task<Print?> GetPublicByIdAsync(int id);

        Task AddAsync(Print print);
        Task UpdateAsync(Print print);
        Task DeleteAsync(Print print);

        Task<bool> ExistsInCollectionAsync(int printId, string userId);
        Task AddToCollectionAsync(UserCollectionPrint entity);
        Task RemoveFromCollectionAsync(int printId, string userId);
        Task<ICollection<Print>> GetCollectionByUserIdAsync(string userId);
        Task<ICollection<Print>> GetLatestPublicPrintsAsync(int count);
        Task<HashSet<int>> GetUserCollectionIdsAsync(string userId);

        Task AddOrUpdateRatingAsync(int printId, string userId, int value);
        Task<int?> GetUserRatingAsync(int printId, string userId);
        Task<Dictionary<int, double>> GetAverageRatingsAsync(IEnumerable<int> printIds);
        Task<Dictionary<int, int>> GetRatingsCountAsync(IEnumerable<int> printIds);
        Task<Dictionary<int, int>> GetUserRatingsForPrintsAsync(IEnumerable<int> printIds, string userId);
    }
}