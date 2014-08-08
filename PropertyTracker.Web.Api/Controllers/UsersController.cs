using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using FluentValidation.Results;
using PropertyTracker.Dto.Validators;
using PropertyTracker.Web.Api.Errors;
using PropertyTracker.Web.Entity.Models;
using PropertyTracker.Web.Api.Routing;
using FluentValidation;

namespace PropertyTracker.Web.Api.Controllers
{
    //[ApiVersion1RoutePrefix("users")]
    [RoutePrefixAttribute("api/users")]
    public class UsersController : ApiController
    {
        private PropertyTrackerContext db = new PropertyTrackerContext();

        // GET: api/Users
        [Route("", Name="GetUsersRoute")]
        [HttpGet]
        [ResponseType(typeof(Dto.Models.UserList))]
        public IHttpActionResult GetUsers()
        {
            var entityUserList = db.Users;
            var userDtoList = Mapper.Map<IEnumerable<Entity.Models.User>, Dto.Models.UserList>(entityUserList);

            ValidationResult userListValidatorResult = new UserListValidator().Validate(userDtoList);
            if (!userListValidatorResult.IsValid)
            {
                throw new HttpResponseException(
                    new ValidatorError("Error mapping user object from database", userListValidatorResult, Request)
                        .Response);
            }

            return Ok(userDtoList);
        }

        // GET: api/Users/5
        [Route("{id:int}", Name="GetUserRoute")]
        [HttpGet]
        [ResponseType(typeof(Dto.Models.User))]
        public IHttpActionResult GetUser(int id)
        {
            Entity.Models.User userEntity = db.Users.Find(id);
            if (userEntity == null)
            {
                return NotFound();
            }

            var userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            ValidationResult userValidatorResult = new UserValidator().Validate(userDto, ruleSet: "default,NoPassword");
            
            if (!userValidatorResult.IsValid)
            {                
                return new ValidatorError("Error mapping user object from database", userValidatorResult, Request);
            }


            return Ok(userDto);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, Dto.Models.User userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var userEntity = Mapper.Map<Dto.Models.User, Entity.Models.User>(userDto);

            db.Entry(userEntity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(Dto.Models.User))]
        public IHttpActionResult PostUser(Dto.Models.User userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = Mapper.Map<Dto.Models.User, Entity.Models.User>(userDto);

            db.Users.Add(userEntity);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userDto.Id }, userDto);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(Dto.Models.User))]
        public IHttpActionResult DeleteUser(int id)
        {
            Entity.Models.User userEntity = db.Users.Find(id);
            if (userEntity == null)
            {
                return NotFound();
            }

            db.Users.Remove(userEntity);
            db.SaveChanges();

            var userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            return Ok(userDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}