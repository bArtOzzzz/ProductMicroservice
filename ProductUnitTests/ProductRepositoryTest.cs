using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Context;
using FluentAssertions;
using Repositories;
using Xunit;
using Moq;

namespace ProductUnitTests
{
    public class ProductRepositoryTest
    {
        protected ProductsRepository _productsRepository;

        public ProductRepositoryTest()
        {
            var optionBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("ProductDb");

            var context = new DataContext(optionBuilder.Options);
            _productsRepository = new ProductsRepository(context);
        }

        //___________________________________Get_All_Async___________________________________
        [Fact]
        protected async Task GetAllAsync_OnSuccess_Return_Success()
        {
            // Act
            var result = await _productsRepository.GetAllAsync();

            // Assert
            result.Should().BeOfType<List<ProductEntity>>();
            result.Should().HaveCount(5);
        }

        //___________________________________Get_By_Id_Async_________________________________
        [Fact]
        protected async Task GetByIdAsync_OnSuccess_Return_Success()
        {
            // Arrange
            var Id = new Guid("b1ee4d3c-99c6-4303-9cba-7241b09034ca");

            // Act
            var result = await _productsRepository.GetByIdAsync(Id);

            // Assert
            result.Should().BeOfType<ProductEntity>();
            result.Name.Should().Be("Milk");
        }

        //___________________________________Create_Async____________________________________
        [Fact]
        protected async Task CreateAsync_OnSuccess_Return_Success()
        {
            // Arrange
            var productEntity = new ProductEntity
            {
                Id = new Guid("28187989-3e1c-4c22-8ea7-4c06f79a3be7"),
                CreatedDate = DateTime.UtcNow,
                LinkImage = "AnotherTestLinkImage",
                Name = "NewTestName"
            };

            // Act
            var result = await _productsRepository.CreateAsync(productEntity);

            // Assert
            result.Should().NotBeEmpty();
        }

        //___________________________________Update_Async_____________________________________
        [Fact]
        protected async Task UpdateAsync_OnSuccess_Return_Success()
        {
            // Arrange
            var productEntity = new ProductEntity
            {
                Id = new Guid("17b170ca-9a3a-4fa8-bfac-0d64472c5174"),
                CreatedDate = DateTime.UtcNow,
                Name = "NewMilkName",
                LinkImage = "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg"
            };

            // Act
            var result = await _productsRepository.UpdateAsync(productEntity.Id, productEntity);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeOfType<string>();
        }

        //___________________________________Delete_Async____________________________________
        [Fact]
        protected async Task DeleteAsync_OnSuccess_Return_Success()
        {
            // Arrange
            var productEntity = new ProductEntity
            {
                Id = new Guid("8a50d3ef-d0c4-42ad-96c3-febc515a17a3"),
                CreatedDate = DateTime.UtcNow,
                Name = "Juice",
                LinkImage = "https://images5.alphacoders.com/102/1022723.jpg"
            };

            // Act
            var result = await _productsRepository.DeleteAsync(productEntity);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        protected async Task DeleteAsync_WhenGetNull_Return_NullReferenceException()
        {
            // AAA
            await _productsRepository.Invoking(y => y.UpdateAsync(It.IsAny<Guid>(), null!))
                                     .Should().ThrowAsync<NullReferenceException>();
        }
    }
}
