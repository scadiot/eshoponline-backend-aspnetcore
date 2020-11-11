using eshoponline.Models;
using Microsoft.EntityFrameworkCore;

namespace eshoponline.Infrastructure
{
    public class EshoponlineContext : DbContext
    {
        public EshoponlineContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CompareGroup> CompareGroups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductKeyword> ProductKeywords { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SpecificationGroup> SpecificationGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ProductCategory>()
            //    .HasOne(pc => pc.Category)
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(pc => pc.CategoryId);
            //
            //modelBuilder.Entity<ProductCategory>()
            //    .HasOne(pc => pc.Product)
            //    .WithMany(p => p.Categories)
            //    .HasForeignKey(pc => pc.ProductId);

            //modelBuilder.Entity<ProductReview>()
            //    .HasOne(pr => pr.Product)
            //    .WithMany(p => p.Reviews)
            //    .HasForeignKey(pr => pr.ProductId);

            //modelBuilder.Entity<ProductReview>()
            //    .HasOne(pr => pr.User)
            //    .WithMany(u => u.ProductReviews)
            //    .HasForeignKey(pr => pr.UserId);
            modelBuilder.Entity<User>()
                .HasMany(u => u.ProductReviews)
                .WithOne(pr => pr.User)
                .IsRequired();

        }
    }
}
