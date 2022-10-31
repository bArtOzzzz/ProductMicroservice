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
        protected readonly IMapper _mapper;

        protected readonly Fixture _fixture = new Fixture();

        protected readonly Mock<IProductsService> _mockProductsService;

        protected ProductsController _productController;

        protected List<ProductDto> _productDtoListFixture;
        protected ProductModel _productModelFixture;

        public ProductController_xUnit()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _productDtoListFixture = _fixture.CreateMany<ProductDto>(3).ToList();
            _productModelFixture = _fixture.Create<ProductModel>();

            _mockProductsService = new Mock<IProductsService>();

            _productController = new ProductsController(_mockProductsService.Object, _mapper);
        }

        //___________________________________Get_All_Async____________________________________
        [Fact]
        protected async Task GetAllAsync_OnSuccess_Return_Ok()
        {
            // Arrange
            var expectedResponse = _mapper.Map<List<ProductResponse>>(_productDtoListFixture);

            _mockProductsService.Setup(config => config.GetAllAsync())
                                .ReturnsAsync(_productDtoListFixture);

            // Act
            var result = (ObjectResult)await _productController.GetAllAsync();

            // Assert
            result.Value.Should().BeOfType<List<ProductResponse>>();
            result.Value.Should().BeEquivalentTo(expectedResponse);
            result.Should().BeOfType<OkObjectResult>();

            _mockProductsService.Verify(p => p.GetAllAsync(), Times.Once);
        }

        [Fact]
        protected async Task GetAllAsync_WhenCollectionIsNull_Return_NotFoundResult()
        {
            // Arrange
            _productDtoListFixture = null!;

            _mockProductsService.Setup(config => config.GetAllAsync())
                                .ReturnsAsync(_productDtoListFixture);

            // Act
            var result = await _productController.GetAllAsync();

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.GetAllAsync(), Times.Once);
        }

        [Fact]
        protected async Task GetAllAsync_WhenCollectionIsEmpty_Return_NotFoundResult()
        {
            // Arrange
            _mockProductsService.Setup(config => config.GetAllAsync())
                                .ReturnsAsync(new List<ProductDto>());

            // Act
            var result = await _productController.GetAllAsync();

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.GetAllAsync(), Times.Once);
        }

        //___________________________________Get_By_Id_Async__________________________________
        [Fact]
        protected async Task GetByIdAsync_OnSuccess_Return_Ok()
        {
            // Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(_productDtoListFixture[0]);

            // Act
            var result = (ObjectResult)await _productController.GetByIdAsync(_productDtoListFixture[0].Id);

            // Assert
            result.Value.Should().BeOfType<ProductResponse>();
            result.Should().BeOfType<OkObjectResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        protected async Task GetByIdAsync_WhenIsNotExist_Return_NotFoundResult()
        {
            // Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(false);

            // Act
            var result = await _productController.GetByIdAsync(Guid.Empty);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        protected async Task GetByIdAsync_WhenNull_Return_NotFoundResult()
        {
            // Arrange
            _productDtoListFixture[0] = null!;

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(_productDtoListFixture[0]);

            // Act
            var result = await _productController.GetByIdAsync(Guid.Empty);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        protected async Task GetByIdAsync_WhenEmpty_Return_NotFoundResult()
        {
            // Arrange
            _mockProductsService.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(new ProductDto());

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            // Act
            var result = await _productController.GetByIdAsync(Guid.Empty);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        //___________________________________Create_Async_____________________________________
        [Fact]
        protected async Task CreateAsync_OnSuccess_Return_CreatedAtActionResult()
        {
            // Arrange
            _mockProductsService.Setup(config => config.CreateAsync(It.IsAny<ProductDto>()))
                                .ReturnsAsync(_productDtoListFixture[0].Id);

            // Act
            var result = (ObjectResult)await _productController.CreateAsync(_productModelFixture);

            // Assert
            result.Value.Should().BeOfType<ProductModel>();
            result.Should().BeOfType<CreatedAtRouteResult>();

            _mockProductsService.Verify(p => p.CreateAsync(It.IsAny<ProductDto>()), Times.Once);
        }

        [Fact]
        protected async Task CreateAsync_WhenNull_Return_NotFoundResult()
        {
            // Arrange
            ProductModel productModel = null!;

            // Act
            var result = await _productController.CreateAsync(productModel);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            
            _mockProductsService.Verify(p => p.CreateAsync(It.IsAny<ProductDto>()), Times.Never);
        }

        [Fact]
        protected async Task CreateAsync_WhenEmpty_Return_NotFoundResult()
        {
            // Act
            var result = await _productController.CreateAsync(new ProductModel());

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.CreateAsync(It.IsAny<ProductDto>()), Times.Never);
        }

        [Fact]
        protected async Task CreateAsync_WhenException_Return_NullReferenceException()
        {
            // Arrange
            _mockProductsService.Setup(config => config.CreateAsync(It.IsAny<ProductDto>()))
                                .ThrowsAsync(new NullReferenceException());

            // Act
            Func<Task> result = async () => await _productController.CreateAsync(_productModelFixture);

            // Assert
            await result.Should().ThrowAsync<NullReferenceException>();

            _mockProductsService.Verify(p => p.CreateAsync(It.IsAny<ProductDto>()), Times.Once);
        }

        //___________________________________Update_Async_____________________________________
        [Fact]
        protected async Task UpdateAsync_OnSuccess_Return_Ok()
        {
            // Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                                .ReturnsAsync(_productDtoListFixture[1].PreviousName);

            // Act
            var result = (ObjectResult)await 
                _productController.UpdateAsync(_productDtoListFixture[1].Id, _productModelFixture);

            // Assert
            result.Value.Should().BeOfType<Guid>();
            result.Should().BeOfType<OkObjectResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Once);
        }

        [Fact]
        protected async Task UpdateAsync_WhenNull_Return_NotFoundResult()
        {
            // Arrange
            ProductModel productModel = null!;

            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            // Act
            var result = await _productController.UpdateAsync(Guid.Empty, productModel);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Never);
        }

        [Fact]
        protected async Task UpdateAsync_WhenEmpty_Return_NotFoundResult()
        {
            // Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            // Act
            var result = await _productController.UpdateAsync(Guid.Empty, new ProductModel());

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Never);
        }

        [Fact]
        protected async Task UpdateAsync_WhenException_Return_NullReferenceException()
        {
            // Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                                .ThrowsAsync(new NullReferenceException());

            // Act
            Func<Task> result = async () => await _productController.UpdateAsync(_productDtoListFixture[1].Id, _productModelFixture);

            // Assert
            await result.Should().ThrowAsync<NullReferenceException>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Once);
        }

        //___________________________________Delete_Async____________________________________
        [Fact]
        protected async Task DeleteAsync_OnSuccess_Return_Ok()
        {
            //Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                                .ReturnsAsync(true);

            // Act
            var result = await _productController.DeleteAsync(It.IsAny<Guid>());

            // Assert
            result.Should().BeOfType<NoContentResult>();

            _mockProductsService.Verify(p => p.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Once);
            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        protected async Task DeleteAsync_WhenNull_Return_NotFoundResult()
        {
            // Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            // Act
            var result = await _productController.DeleteAsync(Guid.Empty);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Once);
        }

        [Fact]
        protected async Task DeleteAsync_WhenException_Return_NullReferenceException()
        {
            // Arrange
            _mockProductsService.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            _mockProductsService.Setup(config => config.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                                .ThrowsAsync(new NullReferenceException());

            // Act
            Func<Task> result = async () => await _productController.DeleteAsync(_productDtoListFixture[1].Id);

            // Assert
            await result.Should().ThrowAsync<NullReferenceException>();

            _mockProductsService.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
            _mockProductsService.Verify(p => p.DeleteAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()), Times.Once);
        }
    }
}
