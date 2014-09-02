using FluentValidation;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Dto.Validators
{
    public class UserListValidator : AbstractValidator<UserList> 
    {
        public UserListValidator()
        {
            // First set the cascade mode
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(list => list.Users).SetCollectionValidator(new UserValidator());
        }
    }
}
