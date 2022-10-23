using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Repositories.Abstract;
using FluentAssertions;
using MassTransit;
using AutoMapper;
using Services;
using Xunit;
using Moq;
using Repositories;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Controllers;
using Services.Dto;
using Repositories.Entities;

namespace ProductUnitTests.Systems.Services
{
    public class ProductTest_Create
    {
        private readonly IMapper _mapper;
        private readonly ProductsService _productsService;
        private readonly FakeRepositoryService _fakeRepositoryService;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

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

            _fakeRepositoryService = new FakeRepositoryService();
            _productsService = new ProductsService(_mockProductRepository.Object, _mockMassTransit.Object, _mapper);
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsRightType()
        {
            // Act
            var result = await _productsService
                .CreateAsync(_fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            result.Should().Be("37d802f6-7782-4abf-a83c-75038942ea40");
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_Verify()
        {
            // Arrange
            _mockProductRepository.Setup(service => service.CreateAsync(It.IsAny<ProductEntity>()))
                               .ReturnsAsync(await _fakeRepositoryService.CreateAsync(_fakeRepositoryService.CreateAsync_OnSuccess));

            // Act
            var result = await _productsService
                .CreateAsync(_fakeRepositoryService.CreateAsync_WhenValidData);

            // Assert
            _mockProductRepository.Verify(p => p.CreateAsync(_fakeRepositoryService.CreateAsync_OnSuccess),
                                               Times.Never(),
                                               "Send was never invoked");
        }
    }
}
