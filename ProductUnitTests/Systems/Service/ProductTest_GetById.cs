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
        private readonly IProductsService _productsService;
        private readonly ProductsController _productsController;

        private readonly Mock<IProductsService> _mockProductService = new();

        public ProductTest_GetById()
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
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            Guid id = new("a1fc0496-1b9a-4023-8e44-ac611652ddf1");

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.GetByIdAsync(id));

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = (OkObjectResult)await _productsController
                .GetByIdAsync(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetByIdAsync_WhenInvalidId_ReturnsStatusCode404()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.GetByIdAsync(id));

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.IsExistAsync(id));

            // Act
            var result = (NotFoundResult)await _productsController
                .GetByIdAsync(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnsRightProduct()
        {
            // Arrange
            Guid testGuid = new("a1fc0496-1b9a-4023-8e44-ac611652ddf1");

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.GetByIdAsync(testGuid));

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(await _productsService.IsExistAsync(testGuid));

            // Act
            var result = (OkObjectResult)await _productsController
                .GetByIdAsync(testGuid);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<ProductResponse>();
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_Verify()
        {
            // Arrange
            Guid Id = new("98691dd5-737f-4610-a842-b029375b0157");

            _mockProductService.Setup(service => service.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.IsExistAsync(Id));

            _mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(await _productsService.GetByIdAsync(Id));

            // Act 
            var result = await _productsController.GetByIdAsync(Id);

            // Assert
            _mockProductService.Verify(p => p.GetByIdAsync(Id),
                                            Times.Once(),
                                            "Send was never invoked");
        }
    }
}
