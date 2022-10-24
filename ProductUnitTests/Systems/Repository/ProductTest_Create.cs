using ProductMicroservice.Mapper;
using ProductUnitTests.Fixtures;
using Repositories.Abstract;
using Repositories.Entities;
using FluentAssertions;
using MassTransit;
using AutoMapper;
using Services;
using Xunit;
using Moq;
using Services.Dto;

namespace ProductUnitTests.Systems.Services
{
    public class ProductTest_Create : IAsyncLifetime
    {
        private readonly IMapper _mapper;
        private readonly ProductsService _productsService;
        private readonly FakeRepositoryService _fakeRepositoryFixture;

        private readonly Mock<IProductsRepository> _mockProductRepository = new();
        private readonly Mock<IPublishEndpoint> _mockMassTransit = new();

        public ProductTest_Create()
        {
            MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new ProductProfile()));
            _mapper = mappingConfig.CreateMapper();

            _fakeRepositoryFixture = new FakeRepositoryService();
            _productsService = new ProductsService(_mockProductRepository.Object, _mockMassTransit.Object, _mapper);
        }

        public async Task DisposeAsync() => 
            await _productsService.CreateAsync(_fakeRepositoryFixture.CreateAsync_WhenValidData);

        public async Task InitializeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsRightType()
        {
            // Act
            var result = await _productsService
                .CreateAsync(_fakeRepositoryFixture.CreateAsync_WhenValidData);

            // Assert
            result.Should().Be("37d802f6-7782-4abf-a83c-75038942ea40");
        }
    }
}
