using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyTracker.Dto.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            
        }

        public User User { get; set; }
        // In future, we may want to return more data.
    }
}
