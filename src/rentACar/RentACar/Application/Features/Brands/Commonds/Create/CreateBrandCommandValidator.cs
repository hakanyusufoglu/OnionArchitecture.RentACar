using FluentValidation;

namespace Application.Features.Brands.Commonds.Create
{
    //CreateBrandCommand bizim isteğimiz
    public class CreateBrandCommandValidator:AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
        }
    }
}
