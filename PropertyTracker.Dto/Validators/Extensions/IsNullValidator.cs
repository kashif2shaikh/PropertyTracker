using FluentValidation.Validators;

namespace PropertyTracker.Dto.Validators.Extensions
{
    public class IsNullValidator : FluentValidation.Validators.PropertyValidator
    {

        public IsNullValidator()
            : base("'{PropertyName}' should be null.")
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return true; // Is Null
            }
            return false;
        }
    }
}
