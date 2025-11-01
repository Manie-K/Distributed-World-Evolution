using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Core.Data
{
    internal class ApplicationDBContext : DbContext
    {
        public DbSet<ModuleDBEntity> Modules => Set<ModuleDBEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var moduleEntity = modelBuilder.Entity<ModuleDBEntity>().ToTable("Modules");

            moduleEntity.HasKey(m => m.ID);
            moduleEntity.Property(m => m.ID).ValueGeneratedOnAdd();
            moduleEntity.Property(m => m.Name).IsRequired();
            moduleEntity.Property(m => m.Type).IsRequired().HasConversion<int>();

            moduleEntity.HasData(
                new ModuleDBEntity(true, "Human module", 5, 5, 5, 100, 100, EntityTypeEnum.Human, 1, new int[] {})
                {
                    ID = -1
                },
                new ModuleDBEntity(true, "Default module", 30, 70, 40, 100, 100, EntityTypeEnum.Animal, 2, new int[] { 101, 201, 307, 401 })
                {
                    ID = -2
                }
            );

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("MODULES_DATABASE_CONNECTION_STRING");

            if (String.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=localhost;Database=appdb;Username=appuser;Password=apppassword";
            }

            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

    }
}
