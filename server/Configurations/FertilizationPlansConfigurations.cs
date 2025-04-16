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
    public class FertilizationPlansConfigurations : IEntityTypeConfiguration<FertilizationPlans>
    {
        public void Configure(EntityTypeBuilder<FertilizationPlans> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.PlantingPlans)
                .WithMany(x => x.FertilizationPlans)
                .HasForeignKey(x => x.Plan_Id);
            builder.HasOne(x => x.Fertilizers)
                .WithMany(x => x.FertilizationPlans)
                .HasForeignKey(x => x.Fertilization_Id);
        }
    }
}