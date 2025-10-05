using AssetManagementApp.Models;

namespace AssetManagementApp.Data.Repositories
{
    public interface IAssetAssignmentRepository
    {
        Task<IEnumerable<AssetAssignment>> GetAllAsync();
        Task<AssetAssignment?> GetByIdAsync(int id);
        Task<AssetAssignment> CreateAsync(AssetAssignment assignment);
        Task<AssetAssignment> UpdateAsync(AssetAssignment assignment);
        Task<bool> DeleteAsync(int id);
        Task<AssetAssignment?> GetActiveAssignmentByAssetIdAsync(int assetId);
        Task<IEnumerable<AssetAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<AssetAssignment>> GetAssignmentsByAssetIdAsync(int assetId);
        Task<bool> ReturnAssetAsync(int assignmentId, string? returnNotes = null);
        Task<IEnumerable<AssetAssignment>> GetActiveAssignmentsAsync();
    }
}