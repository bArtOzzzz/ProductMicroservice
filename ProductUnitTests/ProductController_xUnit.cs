using ProductMicroservice.Models.Response;
using ProductMicroservice.Models.Request;
using ProductMicroservice.Controllers;
using ProductMicroservice.Mapper;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using FluentAssertions;
using Services.Dto;
using AutoFixture;
using AutoMapper;
using Xunit;
using Moq;

namespace ProductUnitTests
{
    public class ProductController_xUnit
    {
        private protected readonly IMapper _mapper;
        private protected Mock<IProductsService> _mockProductsService;
        private protected readonly Fixture _fixture;
        private protected ProductsController _productController = null!;

        public ProductController_xUnit()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _mockProductsService = new Mock<IProductsService>();
        }

        [Fact]
        public async Task GetAllAsync_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var productList = _fixture.CreateMany<ProductDto>(3).ToList();

            _mockProductsService.Setup(config => config.GetAllAsync())
                                .ReturnsAsync(productList);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.GetAllAsync();

            // Assert
            var obj = result as ObjectResult;
            obj.Value.Should().BeOfType<List<ProductResponse>>();

            result.Should().BeOfType<OkObjectResult>();

            _mockProductsService.Verify(p => p.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenException_ReturnsStatusCode404()
        {
            // Arrange
            _mockProductsService.Setup(config => config.GetAllAsync())
                .ReturnsAsync(new List<ProductDto>());

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.GetAllAsync();

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var product = _fixture.Create<ProductDto>();

            _mockProductsService.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(product);

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(true);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.GetByIdAsync(product.Id);

            // Assert
            var obj = result as ObjectResult;
            obj.Value.Should().BeOfType<ProductResponse>();

            result.Should().BeOfType<OkObjectResult>();

            _mockProductsService.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductNotExist_ReturnsStatusCode404()
        {
            // Arrange
            var product = _fixture.Create<ProductDto>();

            _mockProductsService.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(product);

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(false);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.GetByIdAsync(product.Id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsStatusCode201()
        {
            // Arrange
            var productDto = _fixture.Create<ProductDto>();
            var productModel = _fixture.Create<ProductModel>();

            _mockProductsService.Setup(config => config.CreateAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(productDto.Id);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.CreateAsync(productModel);

            // Assert
            var obj = result as ObjectResult;
            obj.Value.Should().BeOfType<ProductModel>();

            result.Should().BeOfType<CreatedAtRouteResult>();

            _mockProductsService.Verify(p => p.CreateAsync(It.IsAny<ProductDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenInvalidData_ReturnsStatusCode404()
        {
            // Arrange
            ProductModel productModel = new() { LinkImage = "TestLinkImage" };

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.CreateAsync(productModel);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.CreateAsync(It.IsAny<ProductDto>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnsStatusCode200()
        {
            var productDto = _fixture.Create<ProductDto>();
            var productModel = _fixture.Create<ProductModel>();

            _mockProductsService.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                .ReturnsAsync(productDto.PreviousName);

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(true);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.UpdateAsync(productDto.Id, productModel);

            // Assert
            var obj = result as ObjectResult;
            obj.Value.Should().BeOfType<Guid>();

            result.Should().BeOfType<OkObjectResult>();

            _mockProductsService.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Once);
            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidData_ReturnsStatusCode404()
        {
            var productDto = _fixture.Create<ProductDto>();
            var productModel = _fixture.Create<ProductModel>();

            _mockProductsService.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                .ReturnsAsync(productDto.PreviousName);

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(false);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            // Act
            var result = await _productController.UpdateAsync(productDto.Id, productModel);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Never);
            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnsStatusCode204()
        {
            _mockProductsService.Setup(config => config.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            var result = await _productController.DeleteAsync(It.IsAny<Guid>());

            result.Should().BeOfType<NoContentResult>();

            _mockProductsService.Verify(p => p.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Once);
            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenExceptions_ReturnsStatusCode404()
        {
            _mockProductsService.Setup(config => config.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(false);

            _productController = new ProductsController(_mockProductsService.Object, _mapper);

            var result = await _productController.DeleteAsync(It.IsAny<Guid>());

            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Never);
            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
