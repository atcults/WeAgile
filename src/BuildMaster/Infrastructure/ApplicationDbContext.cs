using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using BuildMaster.Model;

namespace BuildMaster.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Detaches all of the EntityEntry objects that have been added to the ChangeTracker.
        /// </summary>
        public void DetachAll()
        {
            foreach (var entityEntry in ChangeTracker.Entries().ToArray())
            {
                if (entityEntry.Entity != null)
                {
                    entityEntry.State = EntityState.Detached;
                }
            }
        }

        public DbSet<Configuration> Configurations { get; set; }
    }
}