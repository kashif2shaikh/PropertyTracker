using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PropertyTracker.Web.Entity.Models;
using PropertyTracker.Web.Api.Security;

namespace PropertyTracker.Web.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        public User GetLoggedInUser()
        {
            // User is already logged in - verify.
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                throw new Exception("Consistency issue - user is supposed to be authenticated");
                //return InternalServerError(new Exception("Consistency issue - user is supposed to be authenticated"));
            }

            var userIdentity = HttpContext.Current.User.Identity as UserIdentity;
            if (userIdentity == null || userIdentity.User == null)
            {
                throw new Exception("Consistency issue - user identity not found");
            }
            return userIdentity.User;
        }
    }
}
