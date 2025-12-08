using Microsoft.EntityFrameworkCore;

namespace Server.Core.Data
{
    /// <summary>
    /// Database context for the application, managing ModuleDBEntity objects.
    /// </summary>
    internal class ApplicationDBContext : DbContext
    {
        /// <summary>
        /// Modules table in the database.
        /// </summary>
        public DbSet<ModuleDBEntity> Modules => Set<ModuleDBEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var moduleEntity = modelBuilder.Entity<ModuleDBEntity>().ToTable("Modules");

            moduleEntity.HasKey(m => m.ID);
            moduleEntity.Property(m => m.ID).ValueGeneratedOnAdd();
            moduleEntity.Property(m => m.Name).IsRequired();
            moduleEntity.Property(m => m.Type).IsRequired().HasConversion<int>();

            moduleEntity.HasData(InitialData.GetInitialData());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("MODULES_DATABASE_CONNECTION_STRING");

            if (String.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=localhost:5432;Database=modulesdb;Username=admin;Password=adminpassword";
            }

            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

    }

}