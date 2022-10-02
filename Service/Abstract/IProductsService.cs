using Services.Dto;

namespace Services.Abstract
{
    public interface IProductsService
    {
        //GET
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(Guid id);

        // POST
        Task<Guid> CreateAsync(ProductDto product);

        // PUT
        Task<string> UpdateAsync(Guid productId, ProductDto product);

        // DELETE
        Task<bool> DeleteAsync(Guid id, ProductDto product);

        // EXISTS
        Task<bool> IsExistAsync(Guid productId);
    }
}
