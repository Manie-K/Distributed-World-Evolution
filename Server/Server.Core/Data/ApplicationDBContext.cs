using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Core.Modules;

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
            var connectionString = "abc"; // Example for PostgreSQL
            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

    }
}
