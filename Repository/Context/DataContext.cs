using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
        }

        public DbSet<ProductEntity> Products { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set main settings for entities
            modelBuilder.Entity<ProductEntity>(
                entity =>
                {
                    entity.Property(e => e.Id)
                          .IsRequired();

                    entity.HasIndex(n => n.Name)
                          .IsUnique();
                });

            // SEEDDATA
            modelBuilder.Entity<ProductEntity>().HasData(
                new ProductEntity
                {
                    Id = new Guid("b1ee4d3c-99c6-4303-9cba-7241b09034ca"),
                    CreatedDate = DateTime.UtcNow,
                    Name = "Milk",
                    LinkImage = "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg"
                },

                new ProductEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    Name = "Bread",
                    LinkImage = "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg"
                },

                new ProductEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    Name = "Juice",
                    LinkImage = "https://images5.alphacoders.com/102/1022723.jpg"
                },

                new ProductEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    Name = "Cheese",
                    LinkImage = "https://pm1.narvii.com/6810/05dbd7aaebf3454313b99edfd566b06356a59be3v2_hq.jpg"
                },

                new ProductEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    Name = "Egg",
                    LinkImage = "https://g.foolcdn.com/image/?url=https%3A//g.foolcdn.com/editorial/images/218648/eggs-brown-getty_BSCxkDW.jpg&w=2000&op=resize"
                });
        }
    }
}
