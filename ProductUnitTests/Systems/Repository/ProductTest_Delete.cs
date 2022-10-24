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
using Services.Dto;

namespace ProductUnitTests.Systems.Repository
{
    public class ProductTest_Delete : IAsyncLifetime
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _fakeProductsRepository;
        private readonly ProductsService _productsService;
        private readonly FakeRepositoryService _fakeRepositoryService;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

        public ProductTest_Delete()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeRepositoryService = new FakeRepositoryService();
            _fakeProductsRepository = new FakeRepositoryService();
            _productsService = new ProductsService(_mockProductRepository.Object, _mockMassTransit.Object, _mapper);
        }

        public async Task DisposeAsync() => 
            await _productsService.DeleteAsync(new Guid("a1fc0496-1b9a-4023-8e44-ac611652ddf1"),
                                               _fakeRepositoryService.CreateAsync_WhenValidData);

        public async Task InitializeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnsRightType()
        {
            // Arrange
            Guid id = new("a1fc0496-1b9a-4023-8e44-ac611652ddf1");

            _mockProductRepository.Setup(service => service.DeleteAsync(It.IsAny<ProductEntity>()))
                   .ReturnsAsync(await _fakeProductsRepository.DeleteAsync(_fakeRepositoryService.CreateAsync_OnSuccess));

            // Act
            var result = await _productsService.DeleteAsync(id, _fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            result.Should().Be(true);
        }
    }
}
