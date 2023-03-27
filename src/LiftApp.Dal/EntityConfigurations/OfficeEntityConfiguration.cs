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
    internal class OfficeEntityConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.Id).IsRequired();

            builder.HasOne(o => o.Address)
                .WithOne(a => a.Office)
                .HasForeignKey<Office>(o => o.AddressId)
                .IsRequired();
        }
    }
}
