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
    public class TasksConfigurations : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.PlantingPlans)
                .WithMany(x => x.Tasks);
            builder.HasMany(x => x.Workers)
                .WithMany(x => x.Tasks);
            builder.HasMany(x => x.Equipment)
                .WithMany(x => x.Tasks);
        }
    }
}