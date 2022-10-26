using AutoFixture;
using FluentAssertions;
using ProductUnitTests.Fixtures;
using Repositories;
using Repositories.Entities;
using Xunit;

namespace ProductUnitTests
{
    public class ProductRepository_xUnit : IClassFixture<TestDatabaseFixture>
    {
        private protected readonly Fixture _fixture;

        public ProductRepository_xUnit(TestDatabaseFixture fixture)
        {
            NewContext = fixture;
            _fixture = new Fixture();
        }

        public TestDatabaseFixture NewContext { get; }

        [Fact]
        public async Task GetAllAsync_OnSuccess_ReturnSuccess()
        {
            // Arrange
            using var context = NewContext.CreateContext();
            var controller = new ProductsRepository(context);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            result.Should().BeOfType<List<ProductEntity>>();
            result.Count.Should().Be(7);
        }

        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnSuccess()
        {
            // Arrange
            using var context = NewContext.CreateContext();
            var controller = new ProductsRepository(context);
            var Id = new Guid("8d3ea42a-05f8-4825-95c2-9c2434ed997b");

            // Act
            var result = await controller.GetByIdAsync(Id);

            // Assert
            result.Should().BeOfType<ProductEntity>();
            result.Name.Should().Be("TestName");
        }

        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnSuccess()
        {
            // Arrange
            using var context = NewContext.CreateContext();
            var controller = new ProductsRepository(context);

            var productEntity = new ProductEntity
            {
                Id = new Guid("28187989-3e1c-4c22-8ea7-4c06f79a3be7"),
                CreatedDate = DateTime.UtcNow,
                LinkImage = "AnotherTestLinkImage",
                Name = "NewTestName"
            };

            // Act
            var result = await controller.CreateAsync(productEntity);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnSuccess()
        {
            // Arrange
            using var context = NewContext.CreateContext();
            var controller = new ProductsRepository(context);

            var productEntity = new ProductEntity
            {
                Id = new Guid("b1ee4d3c-99c6-4303-9cba-7241b09034ca"),
                CreatedDate = DateTime.UtcNow,
                Name = "Milk",
                LinkImage = "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg"
            };

            // Act
            var result = await controller.UpdateAsync(productEntity.Id, productEntity);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeOfType<string>();
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnSuccess()
        {
            // Arrange
            using var context = NewContext.CreateContext();
            var controller = new ProductsRepository(context);

            var productEntity = new ProductEntity
            {
                Id = new Guid("b1ee4d3c-99c6-4303-9cba-7241b09034ca"),
                CreatedDate = DateTime.UtcNow,
                Name = "Milk",
                LinkImage = "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg"
            };

            // Act
            var result = await controller.DeleteAsync(productEntity);

            // Assert
            result.Should().BeTrue();
        }
    }
}
