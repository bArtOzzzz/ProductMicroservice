using ProductMicroservice.Mapper;
using Repositories.Abstract;
using Repositories.Entities;
using FluentAssertions;
using Services.Dto;
using MassTransit;
using AutoFixture;
using AutoMapper;
using Services;
using Xunit;
using Moq;

namespace ProductUnitTests
{
    public class ProductService_xUnit
    {
        private protected readonly IMapper _mapper;
        private protected Mock<IProductsRepository> _mockProductsRepository;
        private protected Mock<IPublishEndpoint> _mockPublishEndpoint;
        private protected readonly Fixture _fixture;
        private protected ProductsService _productsService = null!;

        public ProductService_xUnit()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _mockProductsRepository = new Mock<IProductsRepository>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        }

        [Fact]
        public async Task GetAllAsync_OnSuccess_ReturnsRightType()
        {
            // Arrange
            var productList = _fixture.CreateMany<ProductEntity>(3).ToList();

            _mockProductsRepository.Setup(config => config.GetAllAsync())
                                   .ReturnsAsync(productList);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.GetAllAsync();

            // Assert
            result.Should().BeOfType<List<ProductDto>>();

            _mockProductsRepository.Verify(p => p.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnsRightType()
        {
            // Arrange
            var product = _fixture.Create<ProductEntity>();

            _mockProductsRepository.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(product);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.GetByIdAsync(product.Id);

            // Assert
            result.Should().BeOfType<ProductDto>();

            _mockProductsRepository.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenException_ReturnsNull()
        {
            // Arrange
            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.Should().BeNull();
            _mockProductsRepository.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsNotEmptyResult()
        {
            // Arrange
            var product = _fixture.Create<ProductEntity>();
            var productDto = _fixture.Create<ProductDto>();

            _mockProductsRepository.Setup(config => config.CreateAsync(It.IsAny<ProductEntity>()))
                                   .ReturnsAsync(product.Id);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.CreateAsync(productDto);

            // Assert
            result.Should().NotBeEmpty();

            _mockProductsRepository.Verify(p => p.CreateAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenNull_ReturnsRightType()
        {
            // Arrange
            var product = _fixture.Create<ProductEntity>();

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.GetByIdAsync(product.Id);

            // Assert
            result.Should().BeNull();

            _mockProductsRepository.Verify(p => p.CreateAsync(It.IsAny<ProductEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnsRightType()
        {
            //Arrange
            var productDto = _fixture.Create<ProductDto>();
            string testName = "TestName";

            _mockProductsRepository.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()))
                .ReturnsAsync(testName);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.UpdateAsync(productDto.Id, productDto);

            // Assert
            result.Should().BeOfType<string>();

            _mockProductsRepository.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenException_ReturnsNull()
        {
            // Arrange
            var productDto = _fixture.Create<ProductDto>();
            string testName = null!;

            _mockProductsRepository.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()))
                .ReturnsAsync(testName);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.UpdateAsync(productDto.Id, productDto);

            // Assert
            result.Should().BeNull();

            _mockProductsRepository.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnsRightType()
        {
            // Arrange
            var productDto = _fixture.Create<ProductDto>();

            _mockProductsRepository.Setup(config => config.DeleteAsync(It.IsAny<ProductEntity>()))
                                .ReturnsAsync(true);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.DeleteAsync(productDto.Id, productDto);

            // Assert
            result.Should().BeTrue();

            _mockProductsRepository.Verify(p => p.DeleteAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        public async Task IsExistAsync_OnSuccess_ReturnsRightType()
        {
            Guid Id = new("37d802f6-7782-4abf-a83c-75038942ea40");

            _mockProductsRepository.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(true);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.IsExistAsync(Id);

            // Asssert
            result.Should().BeTrue();

            _mockProductsRepository.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task IsExistAsync_WhenNoExist_ReturnsRightType()
        {
            var Id = Guid.Empty;

            _mockProductsRepository.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(false);

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);

            // Act
            var result = await _productsService.IsExistAsync(Id);

            // Asssert
            result.Should().BeFalse();

            _mockProductsRepository.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
