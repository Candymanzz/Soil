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
    public class PlantingPlansConfigurations : IEntityTypeConfiguration<PlantingPlans>
    {
        public void Configure(EntityTypeBuilder<PlantingPlans> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Fields)
                .WithMany(x => x.PlantingPlans)
                .HasForeignKey(x => x.Field_id);
            builder.HasOne(x => x.Crops)
                .WithMany(x => x.PlantingPlans)
                .HasForeignKey(x => x.Crop_id);
            builder.HasOne(x => x.Seasons)
                .WithMany(x => x.PlantingPlans)
                .HasForeignKey(x => x.Season_id);
        }
    }
}