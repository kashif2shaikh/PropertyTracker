using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyTracker.Dto.Validators.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> IsNull<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IsNullValidator());
        }
    }
}
