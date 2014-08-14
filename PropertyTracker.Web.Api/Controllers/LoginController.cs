using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using AutoMapper;
using PropertyTracker.Dto.Models;
using PropertyTracker.Web.Api.Security;
using PropertyTracker.Web.Entity.Models;

namespace PropertyTracker.Web.Api.Controllers
{
    [RoutePrefixAttribute("api/login")]
    [Authorize] // Enforce authorization, so that BasicAuthenticationMessageHandler will verify username/password.
    public class LoginController : ApiController
    {
        private PropertyTrackerContext db = new PropertyTrackerContext();

        // POST: api/login
        [HttpPost]
        [Route("", Name = "LoginRoute")]
        [ResponseType(typeof(Dto.Models.LoginResponse))]
        public IHttpActionResult Login(Dto.Models.LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // User is already logged in - verify.
            if(!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return InternalServerError(new Exception("Consistency issue - user is supposed to be authenticated"));
            }

            var userIdentity = HttpContext.Current.User.Identity as UserIdentity;
            if (userIdentity == null || userIdentity.User == null)
            {
                return InternalServerError(new Exception("Consistency issue - user identity not found"));
            }
            var userEntity = userIdentity.User;
         
            var userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            var loginResponse = new LoginResponse();
            loginResponse.User = userDto;

            return Ok(loginResponse);

        }

        // POST: api/login/logout
        [HttpPost]
        [Route("logout", Name = "LogoutRoute")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Logout()
        {
            //
            // Currently for basic authentication, we don't need to do anything for logging out. 
            // The Principal is cleared automatically and not persisted anywhere.
            //
            // In the future, we could clear out user session.
            //

            /*
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // User not logged in yet - 
                return Ok();
            }

            Thread.CurrentPrincipal = null;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = null;
            }
            */
            return Ok();

        }

      
    }
}
