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
    internal class TimeIntervalEntityConfiguration : IEntityTypeConfiguration<TimeInterval>
    {
        public void Configure(EntityTypeBuilder<TimeInterval> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(ti => ti.Id).ValueGeneratedOnAdd();
            builder.Property(ti => ti.Id).IsRequired();
            builder.Property(ti => ti.DateFrom).IsRequired();
            builder.Property(ti => ti.DateTo).IsRequired();
        }
    }
}
