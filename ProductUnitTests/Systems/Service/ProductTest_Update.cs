using ProductMicroservice.Controllers;
using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using FluentAssertions;
using AutoMapper;
using Xunit;
using Moq;
using ProductMicroservice.Models.Request;

namespace ProductUnitTests.Systems.Controllers
{
    public class ProductTest_Update : IAsyncLifetime
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _fakeProductsService;
        private readonly ProductsController _productsController;
        private readonly FakeProductService _fakeProductsFixture;

        private readonly Mock<IProductsService> _mockProductService = new();

        public ProductTest_Update()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeProductsService = new FakeProductService();
            _fakeProductsFixture = new FakeProductService();
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        public async Task InitializeAsync() => 
            await _productsController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductModel>());

        public async Task DisposeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnsStatusCode201()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = (OkObjectResult)await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidId_ReturnsStatusCode404()
        {
            // Arrange
            Guid Id = Guid.NewGuid();

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _fakeProductsService.IsExistAsync(Id));

            // Act
            var result = (NotFoundResult)await _productsController
                .UpdateAsync(Id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidModelData_ReturnsStatusCode404()
        {
            // Arrange
            Guid id = new("98691dd5-737f-4610-a842-b029375b0157");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = (NotFoundResult)await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenInvalidData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnsUpdatedProduct()
        {
            // Arrange
            Guid id = new("98691dd5-737f-4610-a842-b029375b0157");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = (OkObjectResult)await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Value.Should().BeOfType<Guid>();
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_VerifyIsExistAsync()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = await _productsController
                .UpdateAsync(id, _fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            _mockProductService.Verify(service => service.IsExistAsync(It.IsAny<Guid>()),
                                                  Times.Exactly(2),
                                                  "Send was never invoked");
        }
    }
}
