using ProductMicroservice.Models.Request;
using ProductMicroservice.Controllers;
using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using FluentAssertions;
using Services.Dto;
using AutoMapper;
using Xunit;
using Moq;

namespace ProductUnitTests.Systems.Controllers
{
    public class ProductTest_Create : IAsyncLifetime
    {
        private readonly IMapper _mapper;
        private readonly ProductsController _productsController;
        private readonly FakeProductService _fakeProductsFixture;

        private readonly Mock<IProductsService> _mockProductService = new();

        public ProductTest_Create()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeProductsFixture = new FakeProductService();
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        public async Task InitializeAsync() => 
            await _productsController.CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

        public async Task DisposeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsStatusCode201()
        {
            // Act
            var result = (CreatedAtRouteResult)await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task CreateAsync_WhenInvalidModel_ReturnsStatusCode404()
        {
            // Act
            var result = (NotFoundResult)await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenInvalidData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsEmpty_ReturnsStatusCode404()
        {
            // Act
            var result = (NotFoundResult)await _productsController
                .CreateAsync(new ProductModel());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsProduct()
        {
            // Act
            var result = (CreatedAtRouteResult)await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            // Assert
            result.Value.Should().BeOfType<ProductModel>();
        }

        [Fact]
        public async Task Test()
        {
/*            _mockProductService.Setup(service => service.CreateAsync(It.IsAny<ProductDto>()))
                               .ReturnsAsync(await _productsService.CreateAsync(_fakeProductsFixture.CreateOrUpdateNewProductTest))
                               .Verifiable("Send was never invoked");*/

            var result = (CreatedAtRouteResult)await _productsController
                .CreateAsync(_fakeProductsFixture.CreateOrUpdateAsync_WhenValidData);

            _mockProductService.Verify(p => p.CreateAsync(It.IsAny<ProductDto>()), 
                                                          Times.Exactly(2));

        }
    }
}
