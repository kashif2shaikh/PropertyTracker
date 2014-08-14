using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using PropertyTracker.Web.Entity.Models;

namespace PropertyTracker.Web.Api.Security
{
    public class UserIdentity : GenericIdentity
    {
        public UserIdentity(User user, string type) : base(user.Username,type)
        {
            User = user;
        }

        public User User { get; set; }
    }
}