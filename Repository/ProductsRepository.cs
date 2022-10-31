using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using Repositories.Entities;
using Repositories.Context;

namespace Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DataContext _context;

        // DB
        public ProductsRepository(DataContext context) => _context = context;

        // GET
        public async Task<List<ProductEntity>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking()
                                          .ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Products.Where(p => p.Id.Equals(id))
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync();
        }

        // POST
        public async Task<Guid> CreateAsync(ProductEntity product)
        {
            ProductEntity productEntity = new()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Name = product.Name,
                LinkImage = product.LinkImage
            };

            await _context.AddAsync(productEntity);
            await _context.SaveChangesAsync();

            return productEntity.Id;
        }

        // PUT
        public async Task<string?> UpdateAsync(Guid productId, ProductEntity product)
        {
            var currentProduct = await _context.Products.Where(p => p.Id.Equals(productId))
                                                        .FirstOrDefaultAsync();

            string? productName = currentProduct!.Name;

            currentProduct.LinkImage = product.LinkImage;
            currentProduct.Name = product.Name;

            _context.Update(currentProduct);
            await _context.SaveChangesAsync();

            return productName;
        }

        // DELETE
        public async Task<bool> DeleteAsync(ProductEntity product)
        {
            _context.Remove(product);
            var saved = _context.SaveChangesAsync();

            return await saved > 0;
        }

        // EXISTS
        public async Task<bool> IsExistAsync(Guid productId)
        {
            return await _context.Products.FindAsync(productId) != null;
        }
    }
}
