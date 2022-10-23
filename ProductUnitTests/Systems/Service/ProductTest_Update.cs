using ProductMicroservice.Controllers;
using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using FluentAssertions;
using AutoMapper;
using Xunit;
using Moq;

namespace ProductUnitTests.Systems.Controllers
{
    public class ProductTest_Update
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly ProductsController _productsController;
        private readonly FakeProductService _fakeProductsFixture;

        private readonly Mock<IProductsService> _mockProductService = new();

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

            _productsService = new FakeProductService();
            _fakeProductsFixture = new FakeProductService();
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnsStatusCode201()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = (OkObjectResult)await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidId_ReturnsStatusCode404()
        {
            // Arrange
            Guid Id = Guid.NewGuid();

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _productsService.IsExistAsync(Id));

            // Act
            var result = (NotFoundResult)await _productsController
                .UpdateAsync(Id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidModelData_ReturnsStatusCode404()
        {
            // Arrange
            Guid id = new("98691dd5-737f-4610-a842-b029375b0157");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = (NotFoundResult)await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenInvalidData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnsUpdatedProduct()
        {
            // Arrange
            Guid id = new("98691dd5-737f-4610-a842-b029375b0157");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = (OkObjectResult)await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Guid>();
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_Verify()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            _mockProductService.Verify(service => service.IsExistAsync(id),
                                                  Times.Once(),
                                                  "Send was never invoked");
        }
    }
}
