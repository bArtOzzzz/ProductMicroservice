using System.Diagnostics.CodeAnalysis;

namespace ProductMicroservice.Models.Config
{
    public class ProductsApiOptions
    {
        [ExcludeFromCodeCoverage]
        public string? Endpoint { get; set; }
    }
}
