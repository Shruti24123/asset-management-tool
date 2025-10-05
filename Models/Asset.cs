using System.ComponentModel.DataAnnotations;

namespace AssetManagementApp.Models
{
    public class Asset
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Asset Name")]
        [StringLength(100)]
        public string AssetName { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Asset Type")]
        [StringLength(50)]
        public string AssetType { get; set; } = string.Empty;
        
        [Display(Name = "Make/Model")]
        [StringLength(100)]
        public string? MakeModel { get; set; }
        
        [Required]
        [Display(Name = "Serial Number")]
        [StringLength(100)]
        public string SerialNumber { get; set; } = string.Empty;
        
        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }
        
        [Display(Name = "Warranty Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? WarrantyExpiryDate { get; set; }
        
        [Required]
        public AssetCondition Condition { get; set; } = AssetCondition.New;
        
        [Required]
        public AssetStatus Status { get; set; } = AssetStatus.Available;
        
        [Display(Name = "Is Spare")]
        public bool IsSpare { get; set; }
        
        [Display(Name = "Specifications")]
        public string? Specifications { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<AssetAssignment> AssetAssignments { get; set; } = new List<AssetAssignment>();
    }
    
    public enum AssetCondition
    {
        New,
        Good,
        [Display(Name = "Needs Repair")]
        NeedsRepair,
        Damaged
    }
    
    public enum AssetStatus
    {
        Available,
        Assigned,
        [Display(Name = "Under Repair")]
        UnderRepair,
        Retired
    }
}