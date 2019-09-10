using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace VRS_Base.Models
{
    public partial class IshaMasterContext : DbContext
    {
        public IshaMasterContext()
        {
        }

        public IshaMasterContext(DbContextOptions<IshaMasterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dept> Dept { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=IshaMaster;Username=postgres;Password=iyc@2019");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Dept>(entity =>
            {
                entity.ToTable("dept");

                entity.HasIndex(e => e.DeptName)
                    .HasName("dept_dept_name_key")
                    .IsUnique();

                entity.Property(e => e.DeptId).HasColumnName("dept_id");

                entity.Property(e => e.DeptName)
                    .IsRequired()
                    .HasColumnName("dept_name")
                    .HasMaxLength(255);
            });
        }
    }
}
