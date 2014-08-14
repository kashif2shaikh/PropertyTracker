using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using AutoMapper;
using FluentValidation.Results;
using Ninject.Infrastructure.Language;
using PropertyTracker.Dto.Validators;
using PropertyTracker.Web.Api.Errors;
using PropertyTracker.Web.Entity.Models;
using PropertyTracker.Web.Api.Routing;
using FluentValidation;
using User = PropertyTracker.Dto.Models.User;

namespace PropertyTracker.Web.Api.Controllers
{
    //[ApiVersion1RoutePrefix("users")]
    
    [RoutePrefixAttribute("api/users")]
    [Authorize]
    public class UsersController : ApiController
    {
        private PropertyTrackerContext db = new PropertyTrackerContext();

        // GET: api/Users
        [HttpGet]
        [Route("", Name="GetUserListRoute")]        
        [ResponseType(typeof(Dto.Models.UserList))]
        public IHttpActionResult GetUsers()
        {
            var entityUserList = db.Users;
            var userDtoList = Mapper.Map<IEnumerable<Entity.Models.User>, Dto.Models.UserList>(entityUserList);

            ValidationResult userListValidatorResult = new UserListValidator().Validate(userDtoList, ruleSet: "default,NoPassword");
            if (!userListValidatorResult.IsValid)
            {
                return new ValidatorError("Error mapping user list DTO from database", HttpStatusCode.InternalServerError, userListValidatorResult, Request);
            }

            return Ok(userDtoList);
        }

        // GET: api/Users/5
        [HttpGet]
        [Route("{id:int}", Name="GetUserRoute")]        
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
                return new ValidatorError("Error mapping user DTO from database", HttpStatusCode.InternalServerError, userValidatorResult, Request);
                // I can also do something like this: 
                //return ResponseMessage( new ValidatorError("Error mapping user DTO from database", HttpStatusCode.InternalServerError, userValidatorResult, Request).Response);
            }


            return Ok(userDto);
        }

        // PUT: api/Users/5
        [HttpPut] 
        [Route("{id:int}", Name = "UpdateUserRoute")]
        [ResponseType(typeof(void))]        
        public IHttpActionResult UpdateUser(int id, Dto.Models.User userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userDto == null)
            {
                return new BadRequestErrorMessageResult("Updated user DTO is missing", this);
            }

            ValidationResult userValidatorResult = new UserValidator().Validate(userDto);
            if (!userValidatorResult.IsValid)
            {
                return new ValidatorError("Validation failed for updated user DTO", HttpStatusCode.BadRequest, userValidatorResult, Request);
            }

            if (id != userDto.Id)
            {
                return new BadRequestErrorMessageResult("Updated user DTO id mismatch", this);
            }

            var userEntity = Mapper.Map<Dto.Models.User, Entity.Models.User>(userDto);
            db.Users.Attach(userEntity);
            db.Entry(userEntity).State = EntityState.Modified;

            /* Properties no longer part of User DTO
            db.Entry(userEntity).Collection(u => u.Properties).Load(); // force load         
            var propertyIdList = userDto.Properties;
            var newProperties = db.Properties.Where(p => propertyIdList.Contains(p.Id)).ToList();
            // For this to work, you must load existing Property collection 
            userEntity.Properties = newProperties;
             */

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
        [HttpPost]
        [Route("", Name = "NewUserRoute")]
        [ResponseType(typeof(Dto.Models.User))]
        public IHttpActionResult NewUser(Dto.Models.User userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationResult userValidatorResult = new UserValidator().Validate(userDto, "default,Password");
            if (!userValidatorResult.IsValid)
            {
                return new ValidatorError("Validation failed for new user DTO", HttpStatusCode.BadRequest, userValidatorResult, Request);
            }

            var userEntity = Mapper.Map<Dto.Models.User, Entity.Models.User>(userDto);

            var company = db.Companies.Find(userDto.Company.Id);
            company.Users.Add(userEntity);

            /* Properties no longer part of User DTO
            var propertyIdList = userDto.Properties;
            var properties = db.Properties.Where(p => propertyIdList.Contains(p.Id));
            foreach (var p in properties)
            {
                p.Users.Add(userEntity);
            }
            */
            
            db.SaveChanges();
            
            userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            userValidatorResult = new UserValidator().Validate(userDto, ruleSet: "default,NoPassword");

            if (!userValidatorResult.IsValid)
            {
                return new ValidatorError("Error mapping user DTO from database", HttpStatusCode.InternalServerError, userValidatorResult, Request);
            }

            return CreatedAtRoute("NewUserRoute", new { id = userDto.Id }, userDto);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        [Route("{id:int}", Name = "DeleteUserRoute")]
        [ResponseType(typeof(Dto.Models.User))]
        public IHttpActionResult DeleteUser(int id)
        {
            Entity.Models.User userEntity = db.Users.Find(id);
            if (userEntity == null)
            {
                return NotFound();
            }

            // Get DTO object before deleting or this will fail.
            var userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            ValidationResult userValidatorResult = new UserValidator().Validate(userDto, ruleSet: "default,NoPassword");
            if (!userValidatorResult.IsValid)
            {
                return new ValidatorError("Error mapping user DTO from database", HttpStatusCode.InternalServerError, userValidatorResult, Request);
            }

            // EF diagram won't support cascade deletes on many-to-many relationships, so we have to manually
            // delete user properties here
            foreach (var p in userEntity.Properties)
            {
                p.Users.Remove(userEntity);
            }
            db.Users.Remove(userEntity);
            //db.Entry(userEntity).Collection(u => u.Properties).Load(); // force load
            /*
            foreach (var p in userEntity.Properties)
            {
                p.Users.Remove(userEntity);
            }
            */

            db.SaveChanges();
           
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