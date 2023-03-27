using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiftApp.Dal.EntityConfigurations
{
    internal class MaintenanceEntityConfiguration : IEntityTypeConfiguration<Maintenance>
    {
        public void Configure(EntityTypeBuilder<Maintenance> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Id).IsRequired();
            builder.Property(m => m.Description).IsRequired();

            builder.HasOne(m => m.TimeInterval)
                .WithOne(ti => ti.Maintenance)
                .HasForeignKey<Maintenance>(m => m.TimeIntervalId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Lift>(m => m.Lift)
                .WithMany(l => l.Maintenances)
                .HasForeignKey(m => m.LiftSerialNumber)
                .IsRequired();
        }
    }
}
