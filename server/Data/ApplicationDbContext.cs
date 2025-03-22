using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Field> Fields { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Fertilizer> Fertilizers { get; set; }
        public DbSet<SoilType> SoilTypes { get; set; }
        public DbSet<FertilizationPlan> FertilizationPlans { get; set; }
        public DbSet<FieldAnalysis> FieldAnalyses { get; set; }
        public DbSet<WeatherCondition> WeatherConditions { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FertilizationHistory> FertilizationHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Field>()
                .HasOne(f => f.SoilType)
                .WithMany()
                .HasForeignKey(f => f.SoilTypeId);

            modelBuilder.Entity<FertilizationPlan>()
                .HasOne(fp => fp.Field)
                .WithMany()
                .HasForeignKey(fp => fp.FieldId);

            modelBuilder.Entity<FertilizationPlan>()
                .HasOne(fp => fp.Crop)
                .WithMany()
                .HasForeignKey(fp => fp.CropId);

            modelBuilder.Entity<FertilizationPlan>()
                .HasOne(fp => fp.Fertilizer)
                .WithMany()
                .HasForeignKey(fp => fp.FertilizerId);

            modelBuilder.Entity<FieldAnalysis>()
                .HasOne(fa => fa.Field)
                .WithMany()
                .HasForeignKey(fa => fa.FieldId);

            modelBuilder.Entity<WeatherCondition>()
                .HasOne(wc => wc.Field)
                .WithMany()
                .HasForeignKey(wc => wc.FieldId);

            modelBuilder.Entity<FertilizationHistory>()
                .HasOne(fh => fh.Field)
                .WithMany()
                .HasForeignKey(fh => fh.FieldId);

            modelBuilder.Entity<FertilizationHistory>()
                .HasOne(fh => fh.Fertilizer)
                .WithMany()
                .HasForeignKey(fh => fh.FertilizerId);

            modelBuilder.Entity<FertilizationHistory>()
                .HasOne(fh => fh.User)
                .WithMany()
                .HasForeignKey(fh => fh.UserId);
        }
    }
}