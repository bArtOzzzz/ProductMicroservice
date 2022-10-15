using System.ComponentModel.DataAnnotations;

namespace ProductMicroservice.Models.Request
{
    public class ProductModel
    {
        public string Name { get; set; }
        public string LinkImage { get; set; }
    }
}
