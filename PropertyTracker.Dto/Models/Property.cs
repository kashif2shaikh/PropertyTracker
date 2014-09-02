using System.Collections.Generic;
using Newtonsoft.Json;

namespace PropertyTracker.Dto.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public double SquareFeet { get; set; }
        
        public int CompanyId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<User> Users { get; set; }
        
        //public Company Company { get; set; }
    }
}
