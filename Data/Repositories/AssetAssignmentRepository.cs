using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Models;

namespace AssetManagementApp.Data.Repositories
{
    public class AssetAssignmentRepository : IAssetAssignmentRepository
    {
        private readonly ApplicationDbContext _context;
        
        public AssetAssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetAllAsync()
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }
        
        public async Task<AssetAssignment?> GetByIdAsync(int id)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .FirstOrDefaultAsync(aa => aa.Id == id);
        }
        
        public async Task<AssetAssignment> CreateAsync(AssetAssignment assignment)
        {
            assignment.CreatedAt = DateTime.UtcNow;
            assignment.AssignedDate = DateTime.UtcNow;
            assignment.IsActive = true;
            
            // Update asset status to Assigned
            var asset = await _context.Assets.FindAsync(assignment.AssetId);
            if (asset != null)
            {
                asset.Status = AssetStatus.Assigned;
                asset.UpdatedAt = DateTime.UtcNow;
            }
            
            _context.AssetAssignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }
        
        public async Task<AssetAssignment> UpdateAsync(AssetAssignment assignment)
        {
            _context.Entry(assignment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return assignment;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var assignment = await _context.AssetAssignments.FindAsync(id);
            if (assignment == null)
                return false;
            
            _context.AssetAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<AssetAssignment?> GetActiveAssignmentByAssetIdAsync(int assetId)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .FirstOrDefaultAsync(aa => aa.AssetId == assetId && aa.IsActive && aa.ReturnedDate == null);
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.EmployeeId == employeeId)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetAssignmentsByAssetIdAsync(int assetId)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.AssetId == assetId)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }
        
        public async Task<bool> ReturnAssetAsync(int assignmentId, string? returnNotes = null)
        {
            var assignment = await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .FirstOrDefaultAsync(aa => aa.Id == assignmentId);
            
            if (assignment == null || !assignment.IsActive)
                return false;
            
            assignment.ReturnedDate = DateTime.UtcNow;
            assignment.ReturnNotes = returnNotes;
            assignment.IsActive = false;
            
            // Update asset status back to Available
            if (assignment.Asset != null)
            {
                assignment.Asset.Status = AssetStatus.Available;
                assignment.Asset.UpdatedAt = DateTime.UtcNow;
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<IEnumerable<AssetAssignment>> GetActiveAssignmentsAsync()
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.IsActive && aa.ReturnedDate == null)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }
    }
}