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
    public class FieldsConfigurations : IEntityTypeConfiguration<Fields>
    {
        public void Configure(EntityTypeBuilder<Fields> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}