using AutoMapper;
using FluentAssertions;
using MassTransit;
using Moq;
using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Repositories.Abstract;
using Repositories.Entities;
using Services;
using Xunit;

namespace ProductUnitTests.Systems.Services
{
    public class ProductTest_Update
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;
        private readonly ProductsService _productsService;
        private readonly FakeRepositoryService _fakeRepositoryService;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

        public ProductTest_Update()
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
        public async Task UpdateAsync_WhenValidData_ReturnsRightType()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductRepository.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()))
                                  .ReturnsAsync(await _productsRepository.UpdateAsync(id, _fakeRepositoryService.CreateAsync_OnSuccess));

            // Act
            var result = await _productsService.UpdateAsync(id, _fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<string>();
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_Verify()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductRepository.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()))
                                  .ReturnsAsync(await _productsRepository.UpdateAsync(id, _fakeRepositoryService.CreateAsync_OnSuccess));

            // Act
            var result = await _productsService.UpdateAsync(id, _fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            _mockProductRepository.Verify(p => p.UpdateAsync(id, _fakeRepositoryService.CreateAsync_OnSuccess),
                                               Times.Never(),
                                               "Send was never invoked");
        }
    }
}
