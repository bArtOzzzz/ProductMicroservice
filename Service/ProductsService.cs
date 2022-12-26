using Repositories.Abstract;
using Repositories.Entities;
using Services.Abstract;
using Services.Dto;
using MassTransit;
using AutoMapper;
using MassTransit.Transports;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;
        
        // Azure Queue
        private readonly ISendEndpointProvider _sendEndpointProvider;

        // For Rabbit MQ && Azure topics
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductsService(IProductsRepository productsRepository,
                               ISendEndpointProvider sendEndpointProvider,
                               IPublishEndpoint publishEndpoint,
                               IMapper mapper)
        {
            _productsRepository = productsRepository;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        // GET
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var productsResponseContent = await _productsRepository.GetAllAsync();

            return _mapper.Map<List<ProductDto>>(productsResponseContent);
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

            product.Id = await _productsRepository.CreateAsync(productMap);
              
            product.CrudOperationsInfo = CrudOperationsInfo.Create;
            product.CreatedDate = DateTime.UtcNow;

            // RabbitMq && Azure topics
            // await _publishEndpoint.Publish(product);

            // Azure Service Bus Queue
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("sb://fridgeproduct.servicebus.windows.net/create-product-queue"));
            await sendEndpoint.Send(product);

            return productMap.Id;
        }

        // PUT
        public async Task<string?> UpdateAsync(Guid productId, ProductDto product)
        {
            var productMap = _mapper.Map<ProductEntity>(product);

            product.CrudOperationsInfo = CrudOperationsInfo.Update;
            product.PreviousName = await _productsRepository.UpdateAsync(productId, productMap);

            // RabbitMq && Azure topics
            // await _publishEndpoint.Publish(product);

            // Azure Service Bus Queue
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("sb://fridgeproduct.servicebus.windows.net/update-product-queue"));
            await sendEndpoint.Send(product);

            return product.PreviousName;
        }

        // DELETE
        public async Task<bool> DeleteAsync(Guid productId, ProductDto product)
        {
            var productMap = _mapper.Map<ProductEntity>(product);

            productMap.Id = productId;

            await _productsRepository.DeleteAsync(productMap);

            product.CrudOperationsInfo = CrudOperationsInfo.Delete;

            // RabbitMq && Azure topics
            // await _publishEndpoint.Publish(product);

            // Azure Service Bus Queue
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("sb://fridgeproduct.servicebus.windows.net/delete-product-queue"));
            await sendEndpoint.Send(product);

            return true;
        }

        // EXISTS
        public async Task<bool> IsExistAsync(Guid productId)
        {
            return await _productsRepository.IsExistAsync(productId);
        }
    }
}
