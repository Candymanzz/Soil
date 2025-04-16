using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Configurations;
using server.Models;

namespace server.AppDbContext
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options)
    {
        public DbSet<Crops> Crops { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<FertilizationPlans> FertilizationPlans { get; set; }
        public DbSet<Fertilizers> Fertilizers { get; set; }
        public DbSet<Fields> Fields { get; set; }
        public DbSet<HarvestLogs> HarvestLogs { get; set; }
        public DbSet<PlantingPlans> PlantingPlans { get; set; }
        public DbSet<Seasons> Seasons { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Workers> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CropsConfigurations());
            modelBuilder.ApplyConfiguration(new EquipmentConfigurations());
            modelBuilder.ApplyConfiguration(new FertilizationPlansConfigurations());
            modelBuilder.ApplyConfiguration(new FertilizersConfiguration());
            modelBuilder.ApplyConfiguration(new FieldsConfigurations());
            modelBuilder.ApplyConfiguration(new HarvestLogsConfigurations());
            modelBuilder.ApplyConfiguration(new PlantingPlansConfigurations());
            modelBuilder.ApplyConfiguration(new SeasonsConfigurations());
            modelBuilder.ApplyConfiguration(new TasksConfigurations());
            modelBuilder.ApplyConfiguration(new WorkersConfigurations());

            base.OnModelCreating(modelBuilder);
        }
    }
}
