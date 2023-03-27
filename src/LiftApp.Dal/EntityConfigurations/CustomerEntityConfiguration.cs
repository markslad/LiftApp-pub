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
    internal class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.UseTpcMappingStrategy();

            builder.HasKey(x => x.Identifier);

            builder.Property(c => c.Identifier).IsRequired();
            builder.Property(c => c.PhoneNumber).IsRequired();
            builder.Property(c => c.Email).IsRequired();

            builder.HasOne(c => c.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey<Customer>(c => c.AddressId)
                .IsRequired();
        }
    }
}
