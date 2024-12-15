using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().Property(b => b.Price).HasPrecision(18, 2);

            modelBuilder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);

            modelBuilder.Entity<Publication>().HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<BookSize>().HasQueryFilter(b => !b.IsDeleted);

            modelBuilder.Entity<CoverType>().HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<BookCategory>(x => x.HasKey(b => new { b.BookId, b.CategoryId}));
            modelBuilder.Entity<BookCategory>().HasQueryFilter(b => !b.Category.IsDeleted && !b.Book.IsDeleted);

            modelBuilder.Entity<Bookmark>(x => x.HasKey(bk => new { bk.BookId, bk.UserId }));
            modelBuilder.Entity<Bookmark>().HasQueryFilter(bk => !bk.Book.IsDeleted && !bk.User.IsDeleted);

            modelBuilder.Entity<ShoppingCart>(x => x.HasKey(s => new { s.UserId, s.BookId }));
            modelBuilder.Entity<ShoppingCart>().HasQueryFilter(s => !s.User.IsDeleted && !s.Book.IsDeleted);

            modelBuilder.Entity<ShippingAddress>().HasQueryFilter(sh => !sh.Province.IsDeleted && !sh.City.IsDeleted);

            modelBuilder.Entity<Province>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<City>().HasQueryFilter(c => !c.IsDeleted && !c.Province.IsDeleted);

            modelBuilder.Entity<ShippingMethod>().Property(sh => sh.Cost).HasPrecision(18, 2);


            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publication)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublicationId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<ShippingAddress>()
                .HasOne(sh => sh.Province)
                .WithMany()
                .HasForeignKey(sh => sh.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<ShippingAddress>()
                .HasOne(sh => sh.City)
                .WithMany()
                .HasForeignKey(sh => sh.CityId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<City>()
                .HasOne(c => c.Province)
                .WithMany(p => p.Cities)
                .HasForeignKey(c => c.ProvinceId)
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
                if (entry.State == EntityState.Added)
                    ((IAuditable)entry.Entity).CreatedAt = DateTime.UtcNow;

                ((IAuditable)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<BookSize> BookSizes { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
    }
}
