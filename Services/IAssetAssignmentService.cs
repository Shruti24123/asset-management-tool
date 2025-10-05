using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public interface IAssetAssignmentService
    {
        Task<IEnumerable<AssetAssignment>> GetAllAssignmentsAsync();
        Task<AssetAssignment?> GetAssignmentByIdAsync(int id);
        Task<AssetAssignment> AssignAssetAsync(int assetId, int employeeId, string? notes = null);
        Task<bool> ReturnAssetAsync(int assignmentId, string? returnNotes = null);
        Task<IEnumerable<AssetAssignment>> GetEmployeeAssignmentsAsync(int employeeId);
        Task<IEnumerable<AssetAssignment>> GetAssetHistoryAsync(int assetId);
        Task<AssetAssignment?> GetActiveAssignmentByAssetAsync(int assetId);
        Task<IEnumerable<AssetAssignment>> GetActiveAssignmentsAsync();
        Task<bool> CanAssignAssetAsync(int assetId);
    }
}