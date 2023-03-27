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
    internal class BorrowalEntityConfiguration : IEntityTypeConfiguration<Borrowal>
    {
        public void Configure(EntityTypeBuilder<Borrowal> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Id).IsRequired();
            builder.Property(b => b.PriceADay).IsRequired();

            builder.HasOne(b => b.TimeInterval)
                .WithOne(ti => ti.Borrowal)
                .HasForeignKey<Borrowal>(b => b.TimeIntervalId)
                .IsRequired();

            builder.HasOne<Lift>(b => b.Lift)
                .WithMany(l => l.Borrowals)
                .HasForeignKey(b => b.LiftSerialNumber)
                .IsRequired();

            builder.HasOne<Customer>(b => b.Customer)
                .WithMany(c => c.Borrowals)
                .HasForeignKey(b => b.CustomerIdentifier)
                .IsRequired();
        }
    }
}
