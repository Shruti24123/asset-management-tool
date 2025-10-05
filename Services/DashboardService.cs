using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;

namespace AssetManagementApp.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly string _connectionString;
        
        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        
        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            
            var stats = new DashboardStats();
            
            // Get basic counts
            var sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM Assets) as TotalAssets,
                    (SELECT COUNT(*) FROM Employees WHERE IsActive = 1) as TotalEmployees,
                    (SELECT COUNT(*) FROM Assets WHERE Status = 'Available') as AvailableAssets,
                    (SELECT COUNT(*) FROM Assets WHERE Status = 'Assigned') as AssignedAssets,
                    (SELECT COUNT(*) FROM Assets WHERE Status = 'UnderRepair') as UnderRepairAssets,
                    (SELECT COUNT(*) FROM Assets WHERE Status = 'Retired') as RetiredAssets,
                    (SELECT COUNT(*) FROM Assets WHERE IsSpare = 1) as SpareAssets,
                    (SELECT COUNT(*) FROM Assets 
                     WHERE WarrantyExpiryDate IS NOT NULL 
                     AND WarrantyExpiryDate <= datetime('now', '+30 days')
                     AND WarrantyExpiryDate >= datetime('now')) as AssetsNearingWarrantyExpiry";
            
            var result = await connection.QueryFirstAsync(sql);
            
            stats.TotalAssets = result.TotalAssets;
            stats.TotalEmployees = result.TotalEmployees;
            stats.AvailableAssets = result.AvailableAssets;
            stats.AssignedAssets = result.AssignedAssets;
            stats.UnderRepairAssets = result.UnderRepairAssets;
            stats.RetiredAssets = result.RetiredAssets;
            stats.SpareAssets = result.SpareAssets;
            stats.AssetsNearingWarrantyExpiry = result.AssetsNearingWarrantyExpiry;
            
            // Get assets by type
            var assetTypesSql = "SELECT AssetType, COUNT(*) as Count FROM Assets GROUP BY AssetType";
            var assetTypes = await connection.QueryAsync(assetTypesSql);
            stats.AssetsByType = assetTypes.ToDictionary(x => (string)x.AssetType, x => (int)x.Count);
            
            // Get assets by condition
            var conditionsSql = "SELECT Condition, COUNT(*) as Count FROM Assets GROUP BY Condition";
            var conditions = await connection.QueryAsync(conditionsSql);
            stats.AssetsByCondition = conditions.ToDictionary(x => (string)x.Condition, x => (int)x.Count);
            
            return stats;
        }
        
        public async Task<IEnumerable<dynamic>> GetAssetTypeDistributionAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            
            var sql = @"
                SELECT AssetType, COUNT(*) as Count
                FROM Assets
                GROUP BY AssetType
                ORDER BY Count DESC";
            
            return await connection.QueryAsync(sql);
        }
        
        public async Task<IEnumerable<dynamic>> GetAssetStatusDistributionAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            
            var sql = @"
                SELECT Status, COUNT(*) as Count
                FROM Assets
                GROUP BY Status
                ORDER BY Count DESC";
            
            return await connection.QueryAsync(sql);
        }
        
        public async Task<IEnumerable<dynamic>> GetRecentAssignmentsAsync(int count = 10)
        {
            using var connection = new SqliteConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    aa.Id,
                    aa.AssignedDate,
                    aa.ReturnedDate,
                    aa.IsActive,
                    e.FullName as EmployeeName,
                    e.Department,
                    a.AssetName,
                    a.AssetType,
                    a.SerialNumber
                FROM AssetAssignments aa
                INNER JOIN Employees e ON aa.EmployeeId = e.Id
                INNER JOIN Assets a ON aa.AssetId = a.Id
                ORDER BY aa.AssignedDate DESC
                LIMIT @Count";
            
            return await connection.QueryAsync(sql, new { Count = count });
        }
        
        public async Task<IEnumerable<dynamic>> GetAssetsNearingWarrantyExpiryAsync(int daysFromNow = 30)
        {
            using var connection = new SqliteConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    Id,
                    AssetName,
                    AssetType,
                    SerialNumber,
                    WarrantyExpiryDate,
                    CAST((julianday(WarrantyExpiryDate) - julianday('now')) AS INTEGER) as DaysUntilExpiry
                FROM Assets
                WHERE WarrantyExpiryDate IS NOT NULL
                AND WarrantyExpiryDate <= datetime('now', '+' || @DaysFromNow || ' days')
                AND WarrantyExpiryDate >= datetime('now')
                ORDER BY WarrantyExpiryDate ASC";
            
            return await connection.QueryAsync(sql, new { DaysFromNow = daysFromNow });
        }
    }
}