using ProductMicroservice.Models.Request;
using Services.Abstract;
using Services.Dto;
using System.Xml.Linq;

namespace ProductUnitTests.Fixtures
{
    public class FakeProductService : IProductsService
    {
        public readonly List<ProductDto> fakeProductsList;

        public FakeProductService()
        {
            fakeProductsList = new List<ProductDto>()
            {
                new ProductDto() { Id = new Guid("a1fc0496-1b9a-4023-8e44-ac611652ddf1"), CreatedDate = DateTime.Now,
                    CrudOperationsInfo = CrudOperationsInfo.Create, LinkImage = "TestLinkImage 1",
                    Name = "TestName 1", PreviousName = "PreviousTestName 1"},
                new ProductDto() { Id = new Guid("98691dd5-737f-4610-a842-b029375b0157"), CreatedDate = DateTime.Now,
                    CrudOperationsInfo = CrudOperationsInfo.Create, LinkImage = "TestLinkImage 2",
                    Name = "TestName 2", PreviousName = "PreviousTestName 2"},
                new ProductDto() { Id = new Guid("595823ca-aab8-4889-a6df-944f999b4270"), CreatedDate = DateTime.Now,
                    CrudOperationsInfo = CrudOperationsInfo.Create, LinkImage = "TestLinkImage 3",
                    Name = "TestName 3", PreviousName = "PreviousTestName 3"},
                new ProductDto() { Id = new Guid("0bb24d7f-3532-4ca2-aed8-4da4675c7c37"), CreatedDate = DateTime.Now,
                    CrudOperationsInfo = CrudOperationsInfo.Create, LinkImage = "TestLinkImage 4",
                    Name = "TestName 4", PreviousName = "PreviousTestName 4"}
            };
        }

        // POST DTO
        public ProductDto CreateOrUpdateNewProductTest = new()
        {
            Id = new Guid("a1fc0496-1b9a-4023-8e44-ac611652ddf1"),
            CreatedDate = DateTime.Now,
            CrudOperationsInfo = CrudOperationsInfo.Create,
            LinkImage = "TestLinkImage 1",
            Name = "TestName 1",
            PreviousName = "PreviousTestName 1"
        };

        // POST MODEL
        public ProductModel CreateOrUpdateAsync_WhenValidData = new()
        {
            Name = "Cheese",
            LinkImage = "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg"
        };

        public ProductModel CreateOrUpdateAsync_WhenInvalidData = new()
        {
            LinkImage = "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg"
        };

        // GET
        public Task<List<ProductDto>> GetAllAsync()
        {
            return Task.FromResult(fakeProductsList);
        }

        public Task<ProductDto?> GetByIdAsync(Guid productId)
        {
            var result = fakeProductsList.Where(a => a.Id.Equals(productId))
                                         .FirstOrDefault();

            return Task.FromResult(result);
        }

        // POST
        public Task<Guid> CreateAsync(ProductDto product)
        {
            return Task.FromResult(product.Id);
        }

        // PUT
        public Task<string?> UpdateAsync(Guid productId, ProductDto product)
        {
            return Task.FromResult(product.PreviousName);
        }

        // DELETE
        public Task<bool> DeleteAsync(Guid productId, ProductDto product)
        {
            return Task.FromResult(true);
        }

        // OTHER
        public Task<bool> IsExistAsync(Guid productId)
        {
            var result = fakeProductsList.Where(p => p.Id.Equals(productId));

            bool isExist = false;

            if (result != null && result.Any())
                isExist = true;

            return Task.FromResult(isExist);
        }
    }
}
