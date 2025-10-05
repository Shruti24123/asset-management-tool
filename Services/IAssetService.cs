using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<Asset>> GetAllAssetsAsync();
        Task<Asset?> GetAssetByIdAsync(int id);
        Task<Asset> CreateAssetAsync(Asset asset);
        Task<Asset> UpdateAssetAsync(Asset asset);
        Task<bool> DeleteAssetAsync(int id);
        Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeId = null);
        Task<IEnumerable<Asset>> SearchAssetsAsync(string searchTerm);
        Task<IEnumerable<Asset>> GetAvailableAssetsAsync();
        Task<IEnumerable<Asset>> GetAssetsByTypeAsync(string assetType);
        Task<IEnumerable<Asset>> GetAssetsByStatusAsync(AssetStatus status);
        Task<IEnumerable<Asset>> GetAssetsNearingWarrantyExpiryAsync(int daysFromNow = 30);
    }
}