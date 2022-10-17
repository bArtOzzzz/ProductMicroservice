using ProductMicroservice.Models.Request;
using FluentValidation;

namespace ProductMicroservice.Validation
{
    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {
            RuleFor(p => p.Name).Length(3, 22)
                                .WithMessage("Length should be 3 to 22 characters");

            RuleFor(p => p.LinkImage).Matches(@"^(https?:\/\/)?([\w-]{1,32}\.[\w-]{1,32})[^\s@]*$")
                                     .WithMessage("Incorrect link format");
        }
    }
}
