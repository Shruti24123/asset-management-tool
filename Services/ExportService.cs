using System.Globalization;
using CsvHelper;
using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportAssetsToCsvAsync();
        Task<byte[]> ExportEmployeesToCsvAsync();
        Task<byte[]> ExportAssignmentsToCsvAsync();
    }
    
    public class ExportService : IExportService
    {
        private readonly IAssetService _assetService;
        private readonly IEmployeeService _employeeService;
        private readonly IAssetAssignmentService _assignmentService;
        
        public ExportService(
            IAssetService assetService,
            IEmployeeService employeeService,
            IAssetAssignmentService assignmentService)
        {
            _assetService = assetService;
            _employeeService = employeeService;
            _assignmentService = assignmentService;
        }
        
        public async Task<byte[]> ExportAssetsToCsvAsync()
        {
            var assets = await _assetService.GetAllAssetsAsync();
            
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            
            // Write headers
            csv.WriteField("Asset ID");
            csv.WriteField("Asset Name");
            csv.WriteField("Asset Type");
            csv.WriteField("Make/Model");
            csv.WriteField("Serial Number");
            csv.WriteField("Purchase Date");
            csv.WriteField("Warranty Expiry Date");
            csv.WriteField("Condition");
            csv.WriteField("Status");
            csv.WriteField("Is Spare");
            csv.WriteField("Specifications");
            csv.WriteField("Created Date");
            csv.NextRecord();
            
            // Write data
            foreach (var asset in assets)
            {
                csv.WriteField(asset.Id);
                csv.WriteField(asset.AssetName);
                csv.WriteField(asset.AssetType);
                csv.WriteField(asset.MakeModel ?? "");
                csv.WriteField(asset.SerialNumber);
                csv.WriteField(asset.PurchaseDate?.ToString("yyyy-MM-dd") ?? "");
                csv.WriteField(asset.WarrantyExpiryDate?.ToString("yyyy-MM-dd") ?? "");
                csv.WriteField(asset.Condition.ToString());
                csv.WriteField(asset.Status.ToString());
                csv.WriteField(asset.IsSpare ? "Yes" : "No");
                csv.WriteField(asset.Specifications ?? "");
                csv.WriteField(asset.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                csv.NextRecord();
            }
            
            writer.Flush();
            return memoryStream.ToArray();
        }
        
        public async Task<byte[]> ExportEmployeesToCsvAsync()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            
            // Write headers
            csv.WriteField("Employee ID");
            csv.WriteField("Full Name");
            csv.WriteField("Department");
            csv.WriteField("Email");
            csv.WriteField("Phone Number");
            csv.WriteField("Designation");
            csv.WriteField("Status");
            csv.WriteField("Created Date");
            csv.NextRecord();
            
            // Write data
            foreach (var employee in employees)
            {
                csv.WriteField(employee.Id);
                csv.WriteField(employee.FullName);
                csv.WriteField(employee.Department);
                csv.WriteField(employee.Email);
                csv.WriteField(employee.PhoneNumber ?? "");
                csv.WriteField(employee.Designation);
                csv.WriteField(employee.IsActive ? "Active" : "Inactive");
                csv.WriteField(employee.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                csv.NextRecord();
            }
            
            writer.Flush();
            return memoryStream.ToArray();
        }
        
        public async Task<byte[]> ExportAssignmentsToCsvAsync()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            
            // Write headers
            csv.WriteField("Assignment ID");
            csv.WriteField("Asset Name");
            csv.WriteField("Asset Type");
            csv.WriteField("Serial Number");
            csv.WriteField("Employee Name");
            csv.WriteField("Department");
            csv.WriteField("Assigned Date");
            csv.WriteField("Returned Date");
            csv.WriteField("Status");
            csv.WriteField("Assignment Notes");
            csv.WriteField("Return Notes");
            csv.NextRecord();
            
            // Write data
            foreach (var assignment in assignments)
            {
                csv.WriteField(assignment.Id);
                csv.WriteField(assignment.Asset.AssetName);
                csv.WriteField(assignment.Asset.AssetType);
                csv.WriteField(assignment.Asset.SerialNumber);
                csv.WriteField(assignment.Employee.FullName);
                csv.WriteField(assignment.Employee.Department);
                csv.WriteField(assignment.AssignedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                csv.WriteField(assignment.ReturnedDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "");
                csv.WriteField(assignment.IsActive ? "Active" : "Returned");
                csv.WriteField(assignment.Notes ?? "");
                csv.WriteField(assignment.ReturnNotes ?? "");
                csv.NextRecord();
            }
            
            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}