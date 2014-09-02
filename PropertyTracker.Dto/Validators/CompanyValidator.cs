using FluentValidation;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Dto.Validators
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            // First set the cascade mode
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(company => company.Id).NotEmpty();
            RuleFor(company => company.Name).NotEmpty();
            RuleFor(company => company.Country).NotEmpty();
        }
    }
}
