using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyTracker.Dto.Validators.Extensions
{
    public class IsNullValidator : PropertyValidator
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
