using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PropertyTracker.Dto.Models
{  
    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PhotoUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; } // This should only be populated when user is being updated/added from client. 
                                             // From server, it should NEVER be sent to client!
       
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Company Company { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Property> Properties { get; set; }
    }
}