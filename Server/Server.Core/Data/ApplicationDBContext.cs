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
            var moduleEntity = modelBuilder.Entity<ModuleDBEntity>();

            moduleEntity.HasKey(m => m.ID);
            moduleEntity.Property(m => m.ID).ValueGeneratedOnAdd();
            moduleEntity.Property(m => m.Name).IsRequired();
            moduleEntity.Property(m => m.Type).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("Co tu wpisujemy? Pamietajcie ze to publiczne jest!!!!");
            
            if (String.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=localhost;Database=appdb;Username=appuser;Password=apppassword";
            }

            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

    }
}
