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
    public class ProductServiceTest
    {
        protected readonly IMapper _mapper;

        protected readonly Fixture _fixture = new Fixture();

        protected readonly Mock<IProductsRepository> _mockProductsRepository;
        protected readonly Mock<IPublishEndpoint> _mockPublishEndpoint;

        protected readonly ProductsService _productsService;

        protected List<ProductEntity> _productEnityListFixture;
        protected ProductDto _productDtoFixture;
        protected ProductEntity _productEntityFixture;

        public ProductServiceTest()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _productEnityListFixture = _fixture.CreateMany<ProductEntity>(2).ToList();
            _productDtoFixture = _fixture.Create<ProductDto>();
            _productEntityFixture = _fixture.Create<ProductEntity>();

            _mockProductsRepository = new Mock<IProductsRepository>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();

            _productsService = new ProductsService(_mockProductsRepository.Object, _mockPublishEndpoint.Object, _mapper);
        }

        //___________________________________Get_All_Async___________________________________
        [Fact]
        protected async Task GetAllAsync_OnSuccess_Return_RightCollectionAndType()
        {
            // Arrange
            _mockProductsRepository.Setup(config => config.GetAllAsync())
                                   .ReturnsAsync(_productEnityListFixture);
                
            // Act
            var result = await _productsService.GetAllAsync();

            // Assert
            // TODO: Divide
            result.Should().BeOfType<List<ProductDto>>();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(_productEnityListFixture);

            _mockProductsRepository.Verify(p => p.GetAllAsync(), Times.Once);
        }

        //___________________________________Get_By_Id_Async_________________________________
        [Fact]
        protected async Task GetByIdAsync_OnSuccess_Return_RightObjectAndType()
        {
            // Arrange
            _mockProductsRepository.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(_productEnityListFixture[0]);

            // Act
            var result = await _productsService.GetByIdAsync(_productDtoFixture.Id);

            // Assert
            result.Should().BeOfType<ProductDto>();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_productEnityListFixture[0]);

            _mockProductsRepository.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        protected async Task GetByIdAsync_WhenProductIdIsNotCorrect_Return_Null()
        {
            // Arrange
            _productEntityFixture = null!;

            _mockProductsRepository.Setup(config => config.GetByIdAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(_productEntityFixture);

            // Act
            var result = await _productsService.GetByIdAsync(Guid.Empty);

            // Assert
            result.Should().BeNull();

            _mockProductsRepository.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        //___________________________________Create_Async____________________________________
        [Fact]
        protected async Task CreateAsync_OnSuccess_Return_RightId()
        {
            // Arrange
            _mockProductsRepository.Setup(config => config.CreateAsync(It.IsAny<ProductEntity>()))
                                   .ReturnsAsync(_productDtoFixture.Id);

            // Act
            var result = await _productsService.CreateAsync(_productDtoFixture);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().Be(_productDtoFixture.Id);

            _mockProductsRepository.Verify(p => p.CreateAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        protected async Task CreateAsync_WhenProductIsEmpty_Return_Empty()
        {
            // Arrange
            _mockProductsRepository.Setup(config => config.CreateAsync(It.IsAny<ProductEntity>()))
                                   .ReturnsAsync(Guid.Empty);

            // Act
            var result = await _productsService.CreateAsync(new ProductDto());

            // Assert
            result.Should().BeEmpty();

            _mockProductsRepository.Verify(p => p.CreateAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        protected async Task CreateAsync_WhenGetNull_Return_NullReferenceException()
        {
            // AAA
            await _productsService.Invoking(y => y.CreateAsync(null!))
                                  .Should().ThrowAsync<NullReferenceException>();

            _mockProductsRepository.Verify(p => p.CreateAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        //___________________________________Update_Async_____________________________________
        [Fact]
        protected async Task UpdateAsync_OnSuccess_Return_RightObjectAndType()
        {
            //Arrange
            _mockProductsRepository.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()))
                                   .ReturnsAsync(_productDtoFixture.Name);

            // Act
            var result = await _productsService.UpdateAsync(_productDtoFixture.Id, _productDtoFixture);

            // Assert
            result.Should().BeOfType<string>();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_productDtoFixture.Name);

            _mockProductsRepository.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        protected async Task UpdateAsync_WhenProductIsEmpty_Return_Empty()
        {
            //Arrange
            _mockProductsRepository.Setup(config => config.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()))
                                   .ReturnsAsync(string.Empty);

            // Act
            var result = await _productsService.UpdateAsync(Guid.Empty, new ProductDto());

            // Assert
            result.Should().BeEmpty();

            _mockProductsRepository.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        protected async Task UpdateAsync_WhenGetNull_Return_NullReferenceException()
        {
            // AAA
            await _productsService.Invoking(y => y.UpdateAsync(Guid.Empty ,null!))
                                  .Should().ThrowAsync<NullReferenceException>();

            _mockProductsRepository.Verify(p => p.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductEntity>()), Times.Never);
        }

        //___________________________________Delete_Async____________________________________
        [Fact]
        protected async Task DeleteAsync_OnSuccess_Return_True()
        {
            // Arrange
            _mockProductsRepository.Setup(config => config.DeleteAsync(It.IsAny<ProductEntity>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _productsService.DeleteAsync(_productDtoFixture.Id, _productDtoFixture);

            // Assert
            result.Should().BeTrue();

            _mockProductsRepository.Verify(p => p.DeleteAsync(It.IsAny<ProductEntity>()), Times.Once);
        }

        [Fact]
        protected async Task DeleteAsync_WhenGetNull_Return_NullReferenceException()
        {
            // AAA
            await _productsService.Invoking(y => y.DeleteAsync(Guid.Empty, null!))
                                  .Should().ThrowAsync<NullReferenceException>();

            _mockProductsRepository.Verify(p => p.DeleteAsync(It.IsAny<ProductEntity>()), Times.Never);
        }

        //___________________________________Is_Exist_Async___________________________________
        [Fact]
        protected async Task IsExistAsync_WhenProductExist_Return_True()
        {
            // Arrange
            _mockProductsRepository.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _productsService.IsExistAsync(Guid.NewGuid());

            // Asssert
            result.Should().BeTrue();

            _mockProductsRepository.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        protected async Task IsExistAsync_WhenProductNotExist_Return_False()
        {
            // Arrange
            _mockProductsRepository.Setup(config => config.IsExistAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(false);

            // Act
            var result = await _productsService.IsExistAsync(Guid.Empty);

            // Asssert
            result.Should().BeFalse();

            _mockProductsRepository.Verify(p => p.IsExistAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
