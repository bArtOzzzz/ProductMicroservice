using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Repositories.Abstract;
using FluentAssertions;
using Services.Dto;
using MassTransit;
using AutoMapper;
using Services;
using Xunit;
using Moq;

namespace ProductUnitTests.Systems.Services
{
    public class FakeProductService : IAsyncLifetime
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _fakeProductsRepository;
        private readonly ProductsService _productsService;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

        public FakeProductService()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeProductsRepository = new FakeRepositoryService();
            _productsService = new ProductsService(_mockProductRepository.Object, _mockMassTransit.Object, _mapper);
        }

        public async Task InitializeAsync() => 
            await _productsService.GetByIdAsync(It.IsAny<Guid>());

        public async Task DisposeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task GetByIdAsync_WhenValidData_ReturnsRightType()
        {
            // Arrange
            Guid productId = new("0bb24d7f-3532-4ca2-aed8-4da4675c7c37");

            _mockProductRepository.Setup(service => service.GetByIdAsync(productId))
                               .ReturnsAsync(await _fakeProductsRepository.GetByIdAsync(productId));

            // Act
            var result = await _productsService.GetByIdAsync(productId);

            // Assert
            result.Should().BeOfType<ProductDto>();
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_Verify()
        {
            // Arrange
            Guid Id = new("0bb24d7f-3532-4ca2-aed8-4da4675c7c37");

            _mockProductRepository.Setup(service => service.GetByIdAsync(Id))
                                  .ReturnsAsync(await _fakeProductsRepository.GetByIdAsync(Id));

            // Act
            var result = await _productsService.GetByIdAsync(Id);

            // Assert
            _mockProductRepository.Verify(p => p.GetByIdAsync(Id),
                                               Times.Once(),
                                               "Send was never invoked");
        }
    }
}
