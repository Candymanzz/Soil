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
    public class SeasonsConfigurations : IEntityTypeConfiguration<Seasons>
    {
        public void Configure(EntityTypeBuilder<Seasons> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}