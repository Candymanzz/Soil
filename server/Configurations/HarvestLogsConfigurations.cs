using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations
{
    public class HarvestLogsConfigurations : IEntityTypeConfiguration<HarvestLogs>
    {
        public void Configure(EntityTypeBuilder<HarvestLogs> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.PlantingPlans)
                .WithOne(x => x.HarvestLogs)
                .HasForeignKey<HarvestLogs>(x => x.Plan_Id);
        }
    }
}