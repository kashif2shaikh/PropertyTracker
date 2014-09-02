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
        protected User GetLoggedInUser()
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

        protected void GenerateUserPhotoLink(PropertyTracker.Dto.Models.User userDto)
        {
            //
            // For some reason Url.Content doesn't compute the base url properly for GET "api/users",
            // so we just build it ourselves            
            userDto.PhotoUrl = Url.Content(string.Format("{0}/api/users/{1}/photo", ControllerContext.Configuration.VirtualPathRoot, userDto.Id));
            //userDto.PhotoUrl = Url.Link("GetUserPhotoRoute", userDto.Id); // returns null for GET /api/users

            // If you want a random pic - use this.
            //userDto.PhotoUrl = "http://lorempixel.com/256/256/people/" + userDto.Username; 
        }

        protected void GenerateUserPhotoLinks(IEnumerable<Dto.Models.User> userList)
        {
            foreach (var userDto in userList)
            {
                GenerateUserPhotoLink(userDto);
            }
        }

        protected void GenerateUserPhotoLinks(PropertyTracker.Dto.Models.UserList userDtoList)
        {
            GenerateUserPhotoLinks(userDtoList.Users);            
        }

        protected void GenerateUserPhotoLinks(PropertyTracker.Dto.Models.PropertyList propertyDtoList)
        {
            foreach (var propertyDto in propertyDtoList.Properties)
            {
                GenerateUserPhotoLinks(propertyDto.Users);
            }            
        }

      
    }
}
