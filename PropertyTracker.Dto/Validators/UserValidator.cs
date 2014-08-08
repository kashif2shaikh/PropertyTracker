using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
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
            RuleFor(user => user.Properties).Must(HaveValidIds);

            RuleSet("NoPassword", () => RuleFor(user => user.Password).IsNull());

            RuleSet("Password", () => RuleFor(user => user.Password).NotEmpty());
        }

        private bool HaveValidIds(IList<int> idList)
        {
            return idList.Min() >= 0;
        }
    }
}
