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
    internal class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.Street).IsRequired();
            builder.Property(a => a.HouseNumber).IsRequired();
            builder.Property(a => a.City).IsRequired();
            builder.Property(a => a.ZipCode).IsRequired();
            builder.Property(a => a.Country).IsRequired();
        }
    }
}
