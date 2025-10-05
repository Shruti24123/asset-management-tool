using System.ComponentModel.DataAnnotations;

namespace AssetManagementApp.Models
{
    public class AssetAssignment
    {
        public int Id { get; set; }
        
        [Required]
        public int AssetId { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        [Required]
        [Display(Name = "Assigned Date")]
        [DataType(DataType.DateTime)]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "Returned Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ReturnedDate { get; set; }
        
        [Display(Name = "Assignment Notes")]
        public string? Notes { get; set; }
        
        [Display(Name = "Return Notes")]
        public string? ReturnNotes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public string CreatedBy { get; set; } = "Admin";
        
        // Navigation properties
        public Asset Asset { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }
}