using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Dto.Validators
{
    public class PropertyValidator : AbstractValidator<Property>
    {
        public PropertyValidator()
        {
            // First set the cascade mode
            CascadeMode = CascadeMode.StopOnFirstFailure;
            
        }
    }
}
