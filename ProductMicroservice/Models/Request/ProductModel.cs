using System.ComponentModel.DataAnnotations;

namespace ProductMicroservice.Models.Request
{
    public class ProductModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public string? LinkImage { get; set; }
    }
}
