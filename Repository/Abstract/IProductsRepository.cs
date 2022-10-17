using Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IProductsRepository
    {
        //GET
        Task<List<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(Guid productId);

        // POST
        Task<Guid> CreateAsync(ProductEntity product);

        // PUT
        Task<string> UpdateAsync(Guid productId, ProductEntity product);

        // DELETE
        Task<bool> DeleteAsync(ProductEntity product);

        // EXISTS
        Task<bool> IsExistAsync(Guid productId);
    }
}
