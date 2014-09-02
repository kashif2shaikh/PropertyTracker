using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PropertyTracker.Dto.Models;
using PropertyTracker.Dto.Validators.Extensions;

namespace PropertyTracker.Dto.Validators
{
    public class PropertyListRequestValidator : AbstractValidator<PropertyListRequest>
    {
        public PropertyListRequestValidator()
        {
            // First set the cascade mode
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(req => req.PageSize).GreaterThanOrEqualTo(PropertyListRequest.MinPageSize).LessThanOrEqualTo(PropertyListRequest.MaxPageSize);
            RuleFor(req => req.CurrentPage).GreaterThanOrEqualTo(0);

            RuleFor(req => req.SortColumn).Must(col => (col == PropertyListRequest.CityColumn ||
                                                        col == PropertyListRequest.NameColumn ||
                                                        col == PropertyListRequest.StateColumn))
                                          .Unless(req => String.IsNullOrEmpty(req.SortColumn));
        }

        /*
        private bool HaveValidIds(IList<int> idList)
        {
            return idList.Min() >= 0;
        }
        */
    }
}
