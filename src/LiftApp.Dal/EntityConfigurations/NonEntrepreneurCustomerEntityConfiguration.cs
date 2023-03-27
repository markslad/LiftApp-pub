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
    internal class NonEntrepreneurCustomerEntityConfiguration : IEntityTypeConfiguration<NonEntrepreneurCustomer>
    {
        public void Configure(EntityTypeBuilder<NonEntrepreneurCustomer> builder)
        {
            builder.Property(nec => nec.FirstName).IsRequired();
            builder.Property(nec => nec.Surname).IsRequired();
        }
    }
}
