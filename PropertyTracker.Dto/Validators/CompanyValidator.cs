using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using PropertyTracker.Dto.Models;
using PropertyTracker.Dto.Validators.Extensions;

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
