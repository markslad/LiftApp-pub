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
    internal class OwnAccountWorkerEntityConfiguration : IEntityTypeConfiguration<OwnAccountWorker>
    {
        public void Configure(EntityTypeBuilder<OwnAccountWorker> builder)
        {
            builder.Property(oaw => oaw.FirstName).IsRequired();
            builder.Property(oaw => oaw.Surname).IsRequired();
        }
    }
}
