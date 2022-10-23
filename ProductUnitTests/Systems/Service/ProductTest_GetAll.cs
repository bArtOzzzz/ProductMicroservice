﻿using ProductMicroservice.Models.Response;
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
    public class ProductTest_GetAll
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _fakeProductsService;
        private readonly ProductsController _productsController;

        private readonly Mock<IProductsService> _mockProductService = new();

        public ProductTest_GetAll()
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

            _fakeProductsService = new FakeProductService();
            _productsController = new ProductsController(_mockProductService.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetAllAsync())
                               .ReturnsAsync(await _fakeProductsService.GetAllAsync());

            // Act
            var result = await _productsController.GetAllAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllAsync_WhenListEmpty_ReturnsStatusCode404()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetAllAsync())
                               .ReturnsAsync(It.IsAny<List<ProductDto>>());

            // Act
            var result = await _productsController.GetAllAsync();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllAsync_OnSuccess_ReturnsListOfProducts()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetAllAsync())
                               .ReturnsAsync(await _fakeProductsService.GetAllAsync());

            // Act
            var result = (OkObjectResult)await _productsController.GetAllAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<List<ProductResponse>>();
        }

        [Fact]
        public async Task GetAllAsync_OnSuccess_Verify()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetAllAsync())
                               .ReturnsAsync(await _fakeProductsService.GetAllAsync());

            // Act 
            var result = await _productsController.GetAllAsync();

            // Assert
            _mockProductService.Verify(p => p.GetAllAsync(),
                                            Times.Once(),
                                            "Send was never invoked");
        }
    }
}