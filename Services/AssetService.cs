using AssetManagementApp.Data.Repositories;
using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        
        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
        
        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        {
            return await _assetRepository.GetAllAsync();
        }
        
        public async Task<Asset?> GetAssetByIdAsync(int id)
        {
            return await _assetRepository.GetByIdAsync(id);
        }
        
        public async Task<Asset> CreateAssetAsync(Asset asset)
        {
            // Validate business rules
            if (await _assetRepository.SerialNumberExistsAsync(asset.SerialNumber))
            {
                throw new InvalidOperationException("An asset with this serial number already exists.");
            }
            
            return await _assetRepository.CreateAsync(asset);
        }
        
        public async Task<Asset> UpdateAssetAsync(Asset asset)
        {
            // Validate business rules
            if (await _assetRepository.SerialNumberExistsAsync(asset.SerialNumber, asset.Id))
            {
                throw new InvalidOperationException("An asset with this serial number already exists.");
            }
            
            return await _assetRepository.UpdateAsync(asset);
        }
        
        public async Task<bool> DeleteAssetAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
                return false;
            
            // Check if asset has active assignments
            if (asset.AssetAssignments.Any(aa => aa.IsActive))
            {
                throw new InvalidOperationException("Cannot delete asset with active assignments. Please return the asset first.");
            }
            
            return await _assetRepository.DeleteAsync(id);
        }
        
        public async Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeId = null)
        {
            return !await _assetRepository.SerialNumberExistsAsync(serialNumber, excludeId);
        }
        
        public async Task<IEnumerable<Asset>> SearchAssetsAsync(string searchTerm)
        {
            return await _assetRepository.SearchAsync(searchTerm);
        }
        
        public async Task<IEnumerable<Asset>> GetAvailableAssetsAsync()
        {
            return await _assetRepository.GetAvailableAssetsAsync();
        }
        
        public async Task<IEnumerable<Asset>> GetAssetsByTypeAsync(string assetType)
        {
            return await _assetRepository.GetAssetsByTypeAsync(assetType);
        }
        
        public async Task<IEnumerable<Asset>> GetAssetsByStatusAsync(AssetStatus status)
        {
            return await _assetRepository.GetAssetsByStatusAsync(status);
        }
        
        public async Task<IEnumerable<Asset>> GetAssetsNearingWarrantyExpiryAsync(int daysFromNow = 30)
        {
            var assets = await _assetRepository.GetAllAsync();
            var cutoffDate = DateTime.UtcNow.AddDays(daysFromNow);
            
            return assets.Where(a => a.WarrantyExpiryDate.HasValue && 
                               a.WarrantyExpiryDate.Value <= cutoffDate &&
                               a.WarrantyExpiryDate.Value >= DateTime.UtcNow)
                         .OrderBy(a => a.WarrantyExpiryDate);
        }
    }
}