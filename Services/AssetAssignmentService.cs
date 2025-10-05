using AssetManagementApp.Data.Repositories;
using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public class AssetAssignmentService : IAssetAssignmentService
    {
        private readonly IAssetAssignmentRepository _assignmentRepository;
        private readonly IAssetRepository _assetRepository;
        
        public AssetAssignmentService(
            IAssetAssignmentRepository assignmentRepository,
            IAssetRepository assetRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assetRepository = assetRepository;
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetAllAssignmentsAsync()
        {
            return await _assignmentRepository.GetAllAsync();
        }
        
        public async Task<AssetAssignment?> GetAssignmentByIdAsync(int id)
        {
            return await _assignmentRepository.GetByIdAsync(id);
        }
        
        public async Task<AssetAssignment> AssignAssetAsync(int assetId, int employeeId, string? notes = null)
        {
            // Validate that asset can be assigned
            if (!await CanAssignAssetAsync(assetId))
            {
                throw new InvalidOperationException("Asset is not available for assignment.");
            }
            
            var assignment = new AssetAssignment
            {
                AssetId = assetId,
                EmployeeId = employeeId,
                Notes = notes,
                AssignedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Admin"
            };
            
            return await _assignmentRepository.CreateAsync(assignment);
        }
        
        public async Task<bool> ReturnAssetAsync(int assignmentId, string? returnNotes = null)
        {
            return await _assignmentRepository.ReturnAssetAsync(assignmentId, returnNotes);
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetEmployeeAssignmentsAsync(int employeeId)
        {
            return await _assignmentRepository.GetAssignmentsByEmployeeIdAsync(employeeId);
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetAssetHistoryAsync(int assetId)
        {
            return await _assignmentRepository.GetAssignmentsByAssetIdAsync(assetId);
        }
        
        public async Task<AssetAssignment?> GetActiveAssignmentByAssetAsync(int assetId)
        {
            return await _assignmentRepository.GetActiveAssignmentByAssetIdAsync(assetId);
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetActiveAssignmentsAsync()
        {
            return await _assignmentRepository.GetActiveAssignmentsAsync();
        }
        
        public async Task<bool> CanAssignAssetAsync(int assetId)
        {
            var asset = await _assetRepository.GetByIdAsync(assetId);
            if (asset == null)
                return false;
            
            // Asset must be available and not already assigned
            if (asset.Status != AssetStatus.Available)
                return false;
            
            // Check if there's any active assignment
            var activeAssignment = await _assignmentRepository.GetActiveAssignmentByAssetIdAsync(assetId);
            return activeAssignment == null;
        }
    }
}