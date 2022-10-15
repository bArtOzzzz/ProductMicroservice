using FluentValidation;
using ProductMicroservice.Models.Request;

namespace ProductMicroservice.Validation
{
    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {
            RuleFor(p => p.Name).NotNull().Length(3, 22);
            RuleFor(p => p.LinkImage).NotNull().Matches(@"^(https?:\/\/)?([\w-]{1,32}\.[\w-]{1,32})[^\s@]*$");
        }
    }
}
