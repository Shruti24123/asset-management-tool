namespace AssetManagementApp.Services
{
    public class DashboardStats
    {
        public int TotalAssets { get; set; }
        public int TotalEmployees { get; set; }
        public int AvailableAssets { get; set; }
        public int AssignedAssets { get; set; }
        public int UnderRepairAssets { get; set; }
        public int RetiredAssets { get; set; }
        public int SpareAssets { get; set; }
        public int AssetsNearingWarrantyExpiry { get; set; }
        public Dictionary<string, int> AssetsByType { get; set; } = new();
        public Dictionary<string, int> AssetsByCondition { get; set; } = new();
    }
    
    public interface IDashboardService
    {
        Task<DashboardStats> GetDashboardStatsAsync();
        Task<IEnumerable<dynamic>> GetAssetTypeDistributionAsync();
        Task<IEnumerable<dynamic>> GetAssetStatusDistributionAsync();
        Task<IEnumerable<dynamic>> GetRecentAssignmentsAsync(int count = 10);
        Task<IEnumerable<dynamic>> GetAssetsNearingWarrantyExpiryAsync(int daysFromNow = 30);
    }
}