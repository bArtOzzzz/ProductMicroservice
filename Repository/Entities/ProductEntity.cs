using Repositories.Entities.Abstract;

namespace Repositories.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? LinkImage { get; set; }
    }
}
