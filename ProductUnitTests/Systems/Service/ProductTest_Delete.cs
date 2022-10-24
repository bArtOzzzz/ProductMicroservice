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
    public class ProductTest_Delete
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _fakeProductsService;
        private readonly ProductsController _productsController;
        private readonly FakeProductService _fakeProductsFixture;

        private readonly Mock<IProductsService> _mockProductService = new();

        public ProductTest_Delete()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeProductsFixture = new FakeProductService();
            _fakeProductsService = new FakeProductService();
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnsStatusCode204()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = (NoContentResult)await _productsController
                .DeleteAsync(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task DeleteAsync_WhenInvalidId_ReturnsStatusCode404()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = (NotFoundResult)await _productsController
                .DeleteAsync(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_VerifyIsExistAsync()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = await _productsController
                .DeleteAsync(id);

            // Assert
            _mockProductService.Verify(service => service.IsExistAsync(id),
                                                  Times.Once(),
                                                  "Send was never invoked");
        }
    }
}
