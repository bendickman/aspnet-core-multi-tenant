using FluentValidation;
using MultiTenant.Api.Controllers.Requests;

namespace MultiTenant.Api.Validators
{
    public class CreateProductRequestValidator 
        : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(c => c.Description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(c => c.Rate)
                .NotEmpty();

            RuleFor(c => c.CategoryId)
                .NotEmpty();
        }
    }
}
