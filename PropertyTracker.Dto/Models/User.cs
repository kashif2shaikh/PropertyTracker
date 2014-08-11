using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PropertyTracker.Dto.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; } // This should only be populated when user is being updated/added from client. 
                                             // From server, it should NEVER be sent to client!
        public Company Company { get; set; }

        /*
        public IList<int> Properties { get; set; }
         */
    }
}