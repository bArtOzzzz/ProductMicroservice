using ProductMicroservice.Models.Request;
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
    public class ProductTest_Create
    {
        private readonly IMapper _mapper;
        private readonly ProductsController _productsController;
        private readonly FakeProductService _fakeProductsFixture;

        private readonly Mock<IProductsService> _mockProductService = new();

        public ProductTest_Create()
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
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsStatusCode201()
        {
            // Act
            var result = (CreatedAtRouteResult)await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
        }

        [Fact]
        public async Task CreateAsync_WhenInvalidModel_ReturnsStatusCode404()
        {
            // Act
            var result = (NotFoundResult)await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenInvalidData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsEmpty_ReturnsStatusCode404()
        {
            // Act
            var result = (NotFoundResult)await _productsController
                .CreateAsync(new ProductModel());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsProduct()
        {
            // Act
            var result = (CreatedAtRouteResult)await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
            result.Value.Should().BeOfType<ProductModel>();
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_Verify()
        {
            // Arrange
            ProductDto productDto = new ProductDto()
            {
                Id = new Guid("a1fc0496-1b9a-4023-8e44-ac611652ddf1"),
                CreatedDate = DateTime.Now,
                CrudOperationsInfo = CrudOperationsInfo.Create,
                LinkImage = "TestLinkImage 1",
                Name = "TestName 1",
                PreviousName = "PreviousTestName 1"
            };

            _mockProductService.Setup(service => service.CreateAsync(It.IsAny<ProductDto>()))
                               .ReturnsAsync(await _fakeProductsFixture.CreateAsync(productDto));

            // Act
            var result = await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            _mockProductService.Verify(p => p.CreateAsync(productDto),
                                            Times.Never());
        }
    }
}
