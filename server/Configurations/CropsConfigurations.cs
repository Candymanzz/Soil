using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations
{
    public class CropsConfigurations : IEntityTypeConfiguration<Crops>
    {
        public void Configure(EntityTypeBuilder<Crops> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}