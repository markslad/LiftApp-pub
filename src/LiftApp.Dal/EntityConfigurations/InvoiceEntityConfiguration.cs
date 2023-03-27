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
    internal class InvoiceEntityConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Identifier);

            builder.Property(i => i.Identifier).IsRequired();
            builder.Property(i => i.DateOfIssue).IsRequired();
            builder.Property(i => i.DueDate).IsRequired();
            builder.Property(i => i.DateOfTaxableSupply).IsRequired();
            builder.Property(i => i.Price).IsRequired();
            builder.Property(i => i.Paid).IsRequired();
            builder.Property(i => i.Identifier).IsRequired();
            builder.Property(i => i.Bank).IsRequired();
            builder.Property(i => i.BankAccount).IsRequired();
            builder.Property(i => i.VariableSymbol).IsRequired();

            builder.HasOne<Borrowal>(i => i.Borrowal)
                .WithMany(b => b.Invoices)
                .HasForeignKey(i => i.BorrowalId)
                .IsRequired();
        }
    }
}
