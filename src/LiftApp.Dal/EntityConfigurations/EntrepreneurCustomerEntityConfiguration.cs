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
    internal class EntrepreneurCustomerEntityConfiguration : IEntityTypeConfiguration<EntrepreneurCustomer>
    {
        public void Configure(EntityTypeBuilder<EntrepreneurCustomer> builder)
        {
        }
    }
}
