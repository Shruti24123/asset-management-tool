using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Models;

namespace AssetManagementApp.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }
        
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.AssetAssignments)
                .ThenInclude(aa => aa.Asset)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        
        public async Task<Employee> CreateAsync(Employee employee)
        {
            employee.CreatedAt = DateTime.UtcNow;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
        
        public async Task<Employee> UpdateAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;
            
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email && (excludeId == null || e.Id != excludeId));
        }
        
        public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();
            
            searchTerm = searchTerm.ToLower();
            return await _context.Employees
                .Where(e => e.FullName.ToLower().Contains(searchTerm) ||
                           e.Department.ToLower().Contains(searchTerm) ||
                           e.Email.ToLower().Contains(searchTerm) ||
                           e.Designation.ToLower().Contains(searchTerm))
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }
    }
}