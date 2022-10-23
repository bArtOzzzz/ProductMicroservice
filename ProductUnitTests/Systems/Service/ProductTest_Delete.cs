using ProductMicroservice.Controllers;
using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using FluentAssertions;
using AutoMapper;
using Xunit;
using Moq;
using Services.Dto;

namespace ProductUnitTests.Systems.Controllers
{
    public class ProductTest_Delete
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly ProductsController _productsController;
        private readonly FakeProductService _fakeProductsFixture;

        private readonly Mock<IProductsService> _mockProductService = new();

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

            _fakeProductsFixture = new FakeProductService();
            _productsService = new FakeProductService();
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnsStatusCode204()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = (NoContentResult)await _productsController
                .DeleteAsync(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_WhenInvalidId_ReturnsStatusCode404()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.IsExistAsync(id));

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
                               .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = (NoContentResult)await _productsController
                .DeleteAsync(id);

            // Assert
            _mockProductService.Verify(service => service.IsExistAsync(id),
                                                  Times.Once(),
                                                  "Send was never invoked");
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_VerifyDeleteAsync()
        {
            // Arrange
            Guid id = new("595823ca-aab8-4889-a6df-944f999b4270");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.IsExistAsync(id));

            _mockProductService.Setup(service => service.DeleteAsync(id, _fakeProductsFixture.fakeProductsList[2]))
                               .ReturnsAsync(await _productsService.DeleteAsync(id, _fakeProductsFixture.fakeProductsList[2]));

            // Act
            var result = await _productsController.DeleteAsync(id);

            // Assert
            _mockProductService.Verify(service => service.DeleteAsync(id, _fakeProductsFixture.fakeProductsList[2]),
                                                  Times.Never,
                                                  "Send was never invoked");
        }
    }
}
