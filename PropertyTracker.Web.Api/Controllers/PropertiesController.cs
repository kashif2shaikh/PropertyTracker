using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using FluentValidation.Results;
using PropertyTracker.Dto.Validators;
using PropertyTracker.Web.Api.Errors;
using PropertyTracker.Web.Api.Security;
using PropertyTracker.Web.Entity.Models;

namespace PropertyTracker.Web.Api.Controllers
{
    [RoutePrefixAttribute("api/properties")]
    [Authorize]
    public class PropertiesController : BaseApiController
    {
        private PropertyTrackerContext db = new PropertyTrackerContext();

        // GET: api/Properties
        [HttpGet]
        [Route("", Name = "GetPropertyListRoute")]    
        [ResponseType(typeof(Dto.Models.PropertyList))]
        public IHttpActionResult GetProperties()        
        {
            var userEntity = GetLoggedInUser();

            // Get all properties filtered out by the company based on logged-in user
            var entityPropList = db.Properties.Where(p => p.CompanyId == userEntity.CompanyId);
            var propertyDtoList = Mapper.Map<IEnumerable<Entity.Models.Property>, Dto.Models.PropertyList>(entityPropList);
            
            /* TODO
            ValidationResult userListValidatorResult = new UserListValidator().Validate(userDtoList, ruleSet: "default,NoPassword");
            if (!userListValidatorResult.IsValid)
            {
                return new ValidatorError("Error mapping user list DTO from database", HttpStatusCode.InternalServerError, userListValidatorResult, Request);
            }
            */

            return Ok(propertyDtoList);
        }

        // GET: api/Properties/5
        [HttpGet]
        [Route("{id:int}", Name = "GetPropertyRoute")]     
        [ResponseType(typeof(Dto.Models.Property))]
        public IHttpActionResult GetProperty(int id)
        {
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        // PUT: api/Properties/5
        [HttpPut]
        [Route("{id:int}", Name = "UpdatePropertyRoute")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateProperty(int id, Dto.Models.Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != property.Id)
            {
                return BadRequest();
            }

            db.Entry(property).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
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

        // POST: api/Properties
        [HttpPost]
        [Route("", Name = "NewPropertyRoute")]
        [ResponseType(typeof(Dto.Models.Property))]
        public IHttpActionResult NewProperty(Dto.Models.Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //db.Properties.Add(property);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = property.Id }, property);
        }

        // DELETE: api/Properties/5
        [HttpDelete]
        [Route("{id:int}", Name = "DeletePropertyRoute")]
        [ResponseType(typeof(Dto.Models.Property))]
        public IHttpActionResult DeleteProperty(int id)
        {
            var propertyEntity = db.Properties.Find(id);
            if (propertyEntity == null)
            {
                return NotFound();
            }

            db.Properties.Remove(propertyEntity);
            db.SaveChanges();

            return Ok(propertyEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyExists(int id)
        {
            return db.Properties.Count(e => e.Id == id) > 0;
        }
    }
}