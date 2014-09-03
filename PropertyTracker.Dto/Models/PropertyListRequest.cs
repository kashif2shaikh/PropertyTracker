using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTracker.Dto.Models
{
    // These parameters are passed within the query string, not as JSON
    public class PropertyListRequest
    {
        public const int DefaultPageSize = 10;
        public const int MinPageSize = 1;
        public const int MaxPageSize = 100;

        public const string NameColumn = "Name";
        public const string CityColumn = "City";
        public const string StateColumn = "State";  
              
        // Page Parameters
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } // Must be less than MaxPageSize

        // Search/Filter Parameters
        public string NameFilter { get; set; }
        public string CityFilter { get; set; }
        public string StateFilter { get; set; }

        // Sort Parameters
        public string SortColumn { get; set; }
        public bool SortAscending;
                
        public PropertyListRequest()
        {
            // Default Values if values are missing
            CurrentPage = 0;
            PageSize = DefaultPageSize;

            SortColumn = NameColumn;
            SortAscending = true;

            NameFilter = String.Empty;
            CityFilter = String.Empty;
            StateFilter = String.Empty;
        }
    }
}
