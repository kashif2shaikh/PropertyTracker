using FluentValidation;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Dto.Validators
{
    public class PaginatedPropertyListValidator : PropertyListValidator<PaginatedPropertyList>
    {
        public PaginatedPropertyListValidator()
        {
            RuleFor(p => p.CurrentPage).GreaterThanOrEqualTo(0);
            RuleFor(p => p.PageSize).GreaterThanOrEqualTo(PropertyListRequest.MinPageSize).LessThanOrEqualTo(PropertyListRequest.MaxPageSize);
            RuleFor(p => p.TotalPages).GreaterThanOrEqualTo(0);
            RuleFor(p => p.TotalItems).GreaterThanOrEqualTo(0);
        }
    }
}