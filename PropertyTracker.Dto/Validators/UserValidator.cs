using FluentValidation;
using PropertyTracker.Dto.Models;
using PropertyTracker.Dto.Validators.Extensions;

namespace PropertyTracker.Dto.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            // First set the cascade mode
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(user => user.Id).GreaterThanOrEqualTo(0);
            RuleFor(user => user.Fullname).NotEmpty();
            RuleFor(user => user.Username).NotEmpty();
            RuleFor(user => user.Company).NotNull().SetValidator(new CompanyValidator());
            //RuleFor(user => user.Properties).Must(HaveValidIds).When(user => user.Properties.Count > 0);

            RuleSet("NoPassword", () => ValidatorExtensions.IsNull<User, string>(RuleFor(user => user.Password)));

            RuleSet("Password", () => RuleFor(user => user.Password).NotEmpty());
        }

        /*
        private bool HaveValidIds(IList<int> idList)
        {
            return idList.Min() >= 0;
        }
        */
    }
}
