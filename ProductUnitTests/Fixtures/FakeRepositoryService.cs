using ProductMicroservice.Models.Request;
using Repositories.Abstract;
using Repositories.Entities;
using Services.Dto;

namespace ProductUnitTests.Fixtures
{
    public class FakeRepositoryService : IProductsRepository
    {
        public List<ProductEntity> _productsList;

        public FakeRepositoryService()
        {
            _productsList = new List<ProductEntity>()
            {
                new ProductEntity() { Id = new Guid("a1fc0496-1b9a-4023-8e44-ac611652ddf1"), CreatedDate = DateTime.Now,
                    LinkImage = "TestLinkImage 1", Name = "TestName 1"},
                new ProductEntity() { Id = new Guid("98691dd5-737f-4610-a842-b029375b0157"), CreatedDate = DateTime.Now,
                    LinkImage = "TestLinkImage 2", Name = "TestName 2"},
                new ProductEntity() { Id = new Guid("595823ca-aab8-4889-a6df-944f999b4270"), CreatedDate = DateTime.Now,
                    LinkImage = "TestLinkImage 3", Name = "TestName 3"},
                new ProductEntity() { Id = new Guid("0bb24d7f-3532-4ca2-aed8-4da4675c7c37"), CreatedDate = DateTime.Now,
                    LinkImage = "TestLinkImage 4", Name = "TestName 4"}
            };
        }

        public ProductDto CreateAsync_WhenValidData = new()
        {
            Id = new Guid("37d802f6-7782-4abf-a83c-75038942ea40"),
            CreatedDate = DateTime.Now,
            CrudOperationsInfo = CrudOperationsInfo.Create,
            Name = "Bread",
            PreviousName = "",
            LinkImage = "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg"
        };

        public ProductEntity CreateAsync_OnSuccess = new()
        {
            Id = new Guid("37d802f6-7782-4abf-a83c-75038942ea40"),
            CreatedDate = DateTime.Now,
            Name = "Bread",
            LinkImage = "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg"
        };

        public ProductDto UpdateAsync_WhenUpdate = new()
        {
            Name = "MySweeteUnitTestName",
            LinkImage = "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg"
        };


        public Task<List<ProductEntity>> GetAllAsync()
        {
            return Task.FromResult(_productsList);
        }

        public Task<ProductEntity?> GetByIdAsync(Guid productId)
        {
            var result = _productsList.Where(a => a.Id.Equals(productId))
                                      .FirstOrDefault();

            return Task.FromResult(result);
        }

        public Task<Guid> CreateAsync(ProductEntity product)
        {
            _productsList.Add(product);

            if(_productsList.Count() == 5)
                return Task.FromResult(product.Id);
            else
                return Task.FromResult(Guid.Empty);
        }

        public Task<bool> DeleteAsync(ProductEntity product)
        {
            var existing = _productsList.Where(a => a.Id.Equals(product.Id)).FirstOrDefault();
            _productsList.Remove(existing);

            if(_productsList.Count() == 3)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }

        public Task<bool> IsExistAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateAsync(Guid productId, ProductEntity product)
        {
            return Task.FromResult(product.Name);
        }
    }
}
