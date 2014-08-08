using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyTracker.Dto.Models
{
    public interface IPaginatedList
    {        
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int TotalItems { get; set; }
    }
}
