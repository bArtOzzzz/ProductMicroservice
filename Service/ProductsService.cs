using Repositories.Abstract;
using Repositories.Entities;
using Services.Abstract;
using Services.Dto;
using MassTransit;
using AutoMapper;

namespace Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public ProductsService(IProductsRepository productsRepository,
                               IPublishEndpoint publishEndpoint,
                               IMapper mapper)
        {
            _productsRepository = productsRepository;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        // GET
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productsRepository.GetAllAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid productId)
        {
            var product = await _productsRepository.GetByIdAsync(productId);

            return _mapper.Map<ProductDto>(product);
        }

        // POST
        public async Task<Guid> CreateAsync(ProductDto product)
        {
            var productMap = _mapper.Map<ProductEntity>(product);

            await _productsRepository.CreateAsync(productMap);

            product.CrudOperationsInfo = CrudOperationsInfo.Create;
            product.CreatedDate = DateTime.UtcNow;

            await _publishEndpoint.Publish(product);

            return productMap.Id;
        }

        // PUT
        public async Task<string> UpdateAsync(Guid productId, ProductDto product)
        {
            var productMap = _mapper.Map<ProductEntity>(product);

            product.CrudOperationsInfo = CrudOperationsInfo.Update;
            product.PreviousName = await _productsRepository.UpdateAsync(productId, productMap); ;
            await _publishEndpoint.Publish(product);

            return product.PreviousName;
        }

        // DELETE
        public async Task<bool> DeleteAsync(Guid productId, ProductDto product)
        {
            var productMap = _mapper.Map<ProductEntity>(product);

            productMap.Id = productId;

            await _productsRepository.DeleteAsync(productMap);

            product.CrudOperationsInfo = CrudOperationsInfo.Delete;
            await _publishEndpoint.Publish(product);

            return true;
        }

        // EXISTS
        public async Task<bool> IsExistAsync(Guid productId)
        {
            return await _productsRepository.IsExistAsync(productId);
        }
    }
}
