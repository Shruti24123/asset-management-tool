using AssetManagementApp.Models;

namespace AssetManagementApp.Data.Repositories
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<Asset?> GetByIdAsync(int id);
        Task<Asset> CreateAsync(Asset asset);
        Task<Asset> UpdateAsync(Asset asset);
        Task<bool> DeleteAsync(int id);
        Task<bool> SerialNumberExistsAsync(string serialNumber, int? excludeId = null);
        Task<IEnumerable<Asset>> SearchAsync(string searchTerm);
        Task<IEnumerable<Asset>> GetAvailableAssetsAsync();
        Task<IEnumerable<Asset>> GetAssetsByTypeAsync(string assetType);
        Task<IEnumerable<Asset>> GetAssetsByStatusAsync(AssetStatus status);
    }
}