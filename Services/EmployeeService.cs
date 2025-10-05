using AssetManagementApp.Data.Repositories;
using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }
        
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }
        
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            // Validate business rules
            if (await _employeeRepository.EmailExistsAsync(employee.Email))
            {
                throw new InvalidOperationException("An employee with this email already exists.");
            }
            
            return await _employeeRepository.CreateAsync(employee);
        }
        
        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            // Validate business rules
            if (await _employeeRepository.EmailExistsAsync(employee.Email, employee.Id))
            {
                throw new InvalidOperationException("An employee with this email already exists.");
            }
            
            return await _employeeRepository.UpdateAsync(employee);
        }
        
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return false;
            
            // Check if employee has active assignments
            if (employee.AssetAssignments.Any(aa => aa.IsActive))
            {
                throw new InvalidOperationException("Cannot delete employee with active asset assignments. Please return all assets first.");
            }
            
            return await _employeeRepository.DeleteAsync(id);
        }
        
        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            return !await _employeeRepository.EmailExistsAsync(email, excludeId);
        }
        
        public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            return await _employeeRepository.SearchAsync(searchTerm);
        }
    }
}