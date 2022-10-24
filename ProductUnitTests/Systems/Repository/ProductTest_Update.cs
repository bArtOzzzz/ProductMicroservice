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

namespace ProductUnitTests.Systems.Services
{
    public class ProductTest_Update : IAsyncLifetime
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _fakeProductsRepository;
        private readonly ProductsService _productsService;
        private readonly FakeRepositoryService _fakeRepositoryService;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

        public ProductTest_Update()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeRepositoryService = new FakeRepositoryService();
            _fakeProductsRepository = new FakeRepositoryService();
            _productsService = new ProductsService(_mockProductRepository.Object, _mockMassTransit.Object, _mapper);
        }

        public async Task DisposeAsync() => 
            await _productsService.UpdateAsync(new Guid("595823ca-aab8-4889-a6df-944f999b4270"),
                                               _fakeRepositoryService.CreateAsync_WhenValidData);

        public async Task InitializeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task UpdateAsync_WhenValidData_ReturnsRightType()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductRepository.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()))
                                  .ReturnsAsync(await _fakeProductsRepository.UpdateAsync(id, _fakeRepositoryService.CreateAsync_OnSuccess));

            // Act
            var result = await _productsService.UpdateAsync(id, _fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<string>();
        }
    }
}
