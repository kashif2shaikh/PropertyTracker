using FluentValidation;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Dto.Validators
{
    public class PropertyListValidator<TPropertyList> : AbstractValidator<TPropertyList> where TPropertyList : PropertyList
    {
        public PropertyListValidator()
        {
            // First set the cascade mode
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(list => list.Properties).SetCollectionValidator(new PropertyValidator());
        }
    }
}