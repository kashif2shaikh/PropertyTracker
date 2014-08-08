using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyTracker.Dto.Models
{
    public class PaginatedPropertyList : PropertyList, IPaginatedList
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
