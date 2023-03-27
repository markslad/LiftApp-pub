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
    internal class LiftEntityConfiguration : IEntityTypeConfiguration<Lift>
    {
        public void Configure(EntityTypeBuilder<Lift> builder)
        {
            builder.HasKey(l => l.SerialNumber);

            builder.Property(l => l.SerialNumber).IsRequired();
            builder.Property(l => l.Manufacturer).IsRequired();
            builder.Property(l => l.MaximumHeight).IsRequired();
            builder.Property(l => l.PowerSource).IsRequired();
            builder.Property(l => l.Eliminated).IsRequired();

            builder.HasOne<Office>(l => l.Office)
                .WithMany(o => o.Lifts)
                .HasForeignKey(l => l.OfficeId)
                .IsRequired();
        }
    }
}
