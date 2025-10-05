using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Models;

namespace AssetManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAssignment> AssetAssignments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Department).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Designation).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
            });
            
            // Asset configuration
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AssetName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.AssetType).IsRequired().HasMaxLength(50);
                entity.Property(a => a.MakeModel).HasMaxLength(100);
                entity.Property(a => a.SerialNumber).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Condition).HasConversion<string>();
                entity.Property(a => a.Status).HasConversion<string>();
                entity.HasIndex(a => a.SerialNumber).IsUnique();
            });
            
            // AssetAssignment configuration
            modelBuilder.Entity<AssetAssignment>(entity =>
            {
                entity.HasKey(aa => aa.Id);
                entity.Property(aa => aa.CreatedBy).HasMaxLength(50);
                
                entity.HasOne(aa => aa.Asset)
                    .WithMany(a => a.AssetAssignments)
                    .HasForeignKey(aa => aa.AssetId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(aa => aa.Employee)
                    .WithMany(e => e.AssetAssignments)
                    .HasForeignKey(aa => aa.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}