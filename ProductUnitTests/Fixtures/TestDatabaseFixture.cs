using Microsoft.EntityFrameworkCore;
using Repositories.Context;
using Repositories.Entities;

namespace ProductUnitTests.Fixtures
{
    public class TestDatabaseFixture
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        context.AddRange(
                            new ProductEntity 
                            { 
                                Id = new Guid("8d3ea42a-05f8-4825-95c2-9c2434ed997b"), 
                                CreatedDate = DateTime.UtcNow, 
                                LinkImage = "TestLinkImage", 
                                Name = "TestName" 
                            },
                            new ProductEntity 
                            { 
                                Id= new Guid("96db3b8e-fd53-4835-a98b-0a7f1d4d92b7"),
                                CreatedDate= DateTime.UtcNow,
                                LinkImage = "AnotherTestLinkImage",
                                Name = "AnotherTestName"
                            });

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public DataContext CreateContext()
            => new DataContext(
                new DbContextOptionsBuilder<DataContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);
    }
}
