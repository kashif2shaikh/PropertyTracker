using FluentValidation;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Dto.Validators
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(company => company.Id).NotEmpty();
            RuleFor(company => company.Name).NotEmpty();
            RuleFor(company => company.Country).NotEmpty();
        }
    }
}
