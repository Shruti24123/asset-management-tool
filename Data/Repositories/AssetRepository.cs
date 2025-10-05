using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Models;

namespace AssetManagementApp.Data.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _context;
        
        public AssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }
        
        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.AssetAssignments)
                .ThenInclude(aa => aa.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        
        public async Task<Asset> CreateAsync(Asset asset)
        {
            asset.CreatedAt = DateTime.UtcNow;
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }
        
        public async Task<Asset> UpdateAsync(Asset asset)
        {
            asset.UpdatedAt = DateTime.UtcNow;
            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return asset;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                return false;
            
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> SerialNumberExistsAsync(string serialNumber, int? excludeId = null)
        {
            return await _context.Assets
                .AnyAsync(a => a.SerialNumber == serialNumber && (excludeId == null || a.Id != excludeId));
        }
        
        public async Task<IEnumerable<Asset>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();
            
            searchTerm = searchTerm.ToLower();
            return await _context.Assets
                .Where(a => a.AssetName.ToLower().Contains(searchTerm) ||
                           a.AssetType.ToLower().Contains(searchTerm) ||
                           a.SerialNumber.ToLower().Contains(searchTerm) ||
                           (a.MakeModel != null && a.MakeModel.ToLower().Contains(searchTerm)))
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Asset>> GetAvailableAssetsAsync()
        {
            return await _context.Assets
                .Where(a => a.Status == AssetStatus.Available)
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Asset>> GetAssetsByTypeAsync(string assetType)
        {
            return await _context.Assets
                .Where(a => a.AssetType == assetType)
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Asset>> GetAssetsByStatusAsync(AssetStatus status)
        {
            return await _context.Assets
                .Where(a => a.Status == status)
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }
    }
}