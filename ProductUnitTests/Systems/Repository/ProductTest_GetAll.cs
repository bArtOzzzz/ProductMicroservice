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
using ProductMicroservice.Controllers;

namespace ProductUnitTests.Systems.Services
{
    public class ProductTest_GetAll
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _fakeProductsRepository;
        private readonly ProductsService _productsService;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

        public ProductTest_GetAll()
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

            _fakeProductsRepository = new FakeRepositoryService();
            _productsService = new ProductsService(_mockProductRepository.Object, _mockMassTransit.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_WhenValidData_ReturnsRightType()
        {
            // Arrange
            _mockProductRepository.Setup(service => service.GetAllAsync())
                                  .ReturnsAsync(await _fakeProductsRepository.GetAllAsync());

            // Act
            var result = await _productsService.GetAllAsync();

            // Assert
            result.Should().BeOfType<List<ProductDto>>();
        }

        [Fact]
        public async Task GetAllAsync_OnSuccess_Verify()
        {
            // Arrange
            _mockProductRepository.Setup(service => service.GetAllAsync())
                                  .ReturnsAsync(await _fakeProductsRepository.GetAllAsync());

            // Act 
            var result = await _productsService.GetAllAsync();

            // Assert
            _mockProductRepository.Verify(p => p.GetAllAsync(),
                                               Times.Once(),
                                               "Send was never invoked");
        }
    }
}
