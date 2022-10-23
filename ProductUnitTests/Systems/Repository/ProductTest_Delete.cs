using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Repositories.Abstract;
using Repositories.Entities;
using FluentAssertions;
using MassTransit;
using AutoMapper;
using Services;
using Xunit;
using Moq;

namespace ProductUnitTests.Systems.Repository
{
    public class ProductTest_Delete
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;
        private readonly ProductsService _productsService;
        private readonly FakeRepositoryService _fakeRepositoryService;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

        public ProductTest_Delete()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ProductProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _fakeRepositoryService = new FakeRepositoryService();
            _productsRepository = new FakeRepositoryService();
            _productsService = new ProductsService(_mockProductRepository.Object, _mockMassTransit.Object, _mapper);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnsRightType()
        {
            // Arrange
            Guid id = new("a1fc0496-1b9a-4023-8e44-ac611652ddf1");

            _mockProductRepository.Setup(service => service.DeleteAsync(It.IsAny<ProductEntity>()))
                   .ReturnsAsync(await _productsRepository.DeleteAsync(_fakeRepositoryService.CreateAsync_OnSuccess));

            // Act
            var result = await _productsService.DeleteAsync(id, _fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_Verify()
        {
            // Arrange
            Guid id = new("a1fc0496-1b9a-4023-8e44-ac611652ddf1");

            _mockProductRepository.Setup(service => service.DeleteAsync(It.IsAny<ProductEntity>()))
                   .ReturnsAsync(await _productsRepository.DeleteAsync(_fakeRepositoryService.CreateAsync_OnSuccess));

            // Act
            var result = await _productsService.DeleteAsync(id, _fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            _mockProductRepository.Verify(p => p.DeleteAsync(_fakeRepositoryService.CreateAsync_OnSuccess),
                                               Times.Never(),
                                               "Send was never invoked");
        }
    }
}
