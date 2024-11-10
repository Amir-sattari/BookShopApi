using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().Property(b => b.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted && !b.Publication.IsDeleted);

            modelBuilder.Entity<Publication>().HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<BookSize>().HasQueryFilter(b => !b.IsDeleted);

            modelBuilder.Entity<CoverType>().HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publication)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublicationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            ApplySoftDelete();
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplySoftDelete();
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplySoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted && e.Entity is IDeleteable))
            {
                entry.State = EntityState.Modified;
                ((IDeleteable)entry.Entity).IsDeleted = true;
                ((IDeleteable)entry.Entity).DeletedAt = DateTime.UtcNow;
            }
        }

        private void ApplyAuditInformation()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) && e.Entity is IAuditable))
            {
                if(entry.State == EntityState.Added)
                    ((IAuditable)entry.Entity).CreatedAt = DateTime.UtcNow;

                ((IAuditable)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<BookSize> BookSizes { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
    }
}
