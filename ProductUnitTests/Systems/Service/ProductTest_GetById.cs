using ProductMicroservice.Models.Response;
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
    public class ProductTest_GetById
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _fakeProductsService;
        private readonly ProductsController _productsController;

        private readonly Mock<IProductsService> _mockProductService = new();

        public ProductTest_GetById()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeProductsService = new FakeProductService();
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            Guid id = new("a1fc0496-1b9a-4023-8e44-ac611652ddf1");

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.GetByIdAsync(id));

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = (OkObjectResult)await _productsController
                .GetByIdAsync(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByIdAsync_WhenInvalidId_ReturnsStatusCode404()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.GetByIdAsync(id));

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.IsExistAsync(id));

            // Act
            var result = (NotFoundResult)await _productsController
                .GetByIdAsync(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnsRightProduct()
        {
            // Arrange
            Guid testGuid = new("a1fc0496-1b9a-4023-8e44-ac611652ddf1");

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.GetByIdAsync(testGuid));

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(await _fakeProductsService.IsExistAsync(testGuid));

            // Act
            var result = (OkObjectResult)await _productsController
                .GetByIdAsync(testGuid);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<ProductResponse>();
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_VerifyGetByIdAsync()
        {
            // Arrange
            Guid Id = new("98691dd5-737f-4610-a842-b029375b0157");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.IsExistAsync(Id));

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.GetByIdAsync(Id));

            // Act 
            var result = await _productsController.GetByIdAsync(Id);

            // Assert
            _mockProductService.Verify(p => p.GetByIdAsync(Id),
                                            Times.Once(),
                                            "Send was never invoked");
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_VerifyIsExistAsync()
        {
            // Arrange
            Guid Id = new("98691dd5-737f-4610-a842-b029375b0157");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.IsExistAsync(Id));

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _fakeProductsService.GetByIdAsync(Id));

            // Act 
            var result = await _productsController.GetByIdAsync(Id);

            // Assert
            _mockProductService.Verify(p => p.IsExistAsync(Id),
                                            Times.Once(),
                                            "Send was never invoked");
        }
    }
}
