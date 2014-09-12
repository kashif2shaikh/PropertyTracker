using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using AutoMapper;
using FluentValidation.Results;
using Org.BouncyCastle.Security;
using PropertyTracker.Dto.Models;
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
        private Entity.Models.User loggedInUser = null;

        // GET: api/Properties
        [HttpGet]
        [Route("", Name = "GetPropertyListRoute")]    
        [ResponseType(typeof(Dto.Models.PaginatedPropertyList))]
        public IHttpActionResult GetProperties([FromUri] PropertyListRequest requestParams)       
        {
            loggedInUser = GetLoggedInUser();

            if (requestParams == null)
            {
                return BadRequest("Property list request parameters are missing");
            }

            
            var result = new PropertyListRequestValidator().Validate(requestParams);
            if (!result.IsValid)
            {
                return new ValidatorError("Error validating page request parameters", HttpStatusCode.BadRequest, result, Request);
            }
                 
            // Get properties based on parameters we got
            var paginatedList = GetPaginatedPropertyList(requestParams);

            return Ok(paginatedList);
        }

        // #future: If we want to build on this query - make return type IQueryable<>
        private IQueryable<Entity.Models.Property> PropertiesQuery(PropertyListRequest requestParams = null)
        {
            // Only return properties that logged in user belongs to.
            var query = db.Properties.Where(p => p.CompanyId == loggedInUser.CompanyId);

            if (requestParams != null)
            {
                // First apply search filter
                if (!String.IsNullOrEmpty(requestParams.StateFilter))
                {
                    var stateFilter = requestParams.StateFilter.Trim().ToLower();
                    query = query.Where(p => (!String.IsNullOrEmpty(p.StateProvince) && p.StateProvince.ToLower().Contains(stateFilter)));
                }

                if (!String.IsNullOrEmpty(requestParams.CityFilter))
                {
                    var cityFilter = requestParams.CityFilter.Trim().ToLower();
                    query = query.Where(p => (!String.IsNullOrEmpty(p.City) && p.City.ToLower().Contains(cityFilter)));
                }                              
                
                if (!String.IsNullOrEmpty(requestParams.NameFilter))
                {
                    var nameFilter = requestParams.NameFilter.Trim().ToLower();
                    query = query.Where(p => (!String.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(nameFilter)));
                }
                    
                // Second apply sorting
                if (!String.IsNullOrEmpty(requestParams.SortColumn))
                {
                    bool asc = requestParams.SortAscending;
                    switch (requestParams.SortColumn)
                    {
                        case PropertyListRequest.NameColumn:
                            query = asc ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                            break;
                        case PropertyListRequest.CityColumn:
                            query = asc ? query.OrderBy(p => p.City) : query.OrderByDescending(p => p.City);
                            break;
                        case PropertyListRequest.StateColumn:
                            query = asc ? query.OrderBy(p => p.StateProvince) : query.OrderByDescending(p => p.StateProvince);
                            break;
                    }
                }
                else
                {
                    // Skip Needs orderby clause
                    query = query.OrderBy(p => p.Name);
                }
            }
            return query;
        }

        private PaginatedPropertyList GetPaginatedPropertyList(PropertyListRequest requestParams)
        {
            var query = PropertiesQuery(requestParams);
            
            var totalItems = query.Count();
            
            // if no limit for page size, then page size will be equal to total items so we return all results.
            var pageSize = requestParams.PageSize != PropertyListRequest.NoLimitForPageSize ? requestParams.PageSize : totalItems;

            query = query.Skip(requestParams.CurrentPage * pageSize).Take(pageSize);
                                       
            var propertyDtoList = Mapper.Map<IQueryable<Entity.Models.Property>, List<Dto.Models.Property>>(query);
            GenerateUserPhotoLinks(propertyDtoList);

            // If requestParams is not empty, we should have received page info.
            var paginatedList = new PaginatedPropertyList
            {
                Properties = propertyDtoList,
                CurrentPage = requestParams.CurrentPage,
                PageSize =  pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                TotalItems = totalItems
            };
            return paginatedList;
        }


        /* Obsolete: Properties are now returned part of User DTO
            // GET: api/users/5/properties
        [HttpGet]
        [Route("~/api/users/{userId:int}/properties", Name = "GetPropertiesByUserRoute")]
        [ResponseType(typeof(Dto.Models.PropertyList))]
        public IHttpActionResult GetPropertiesByUser(int userId)
        {
            loggedInUser = GetLoggedInUser();

            var userEntity = db.Users.Find(userId);
            if (userEntity == null)
            {
                return NotFound();
            }

            if (userEntity.CompanyId != loggedInUser.CompanyId)
            {
                return BadRequest("Requested user does not belong to same company as logged in user");
            }            
                      
            // Fortunately, our we have a relationship be user and associated properties, so this is easy.
            var propertyDtoList = Mapper.Map<IEnumerable<Entity.Models.Property>, Dto.Models.PropertyList>(userEntity.Properties);
            GenerateUserPhotoLinks(propertyDtoList);

            var result = new PropertyListValidator<Dto.Models.PropertyList>().Validate(propertyDtoList);
            if (!result.IsValid)
            {
                return new ValidatorError("Error mapping property list DTO from database", HttpStatusCode.InternalServerError, result, Request);
            }           

            return Ok(propertyDtoList);
        }
        */

        // GET: api/Properties/5
        [HttpGet]
        [Route("{id:int}", Name = "GetPropertyRoute")]     
        [ResponseType(typeof(Dto.Models.Property))]
        public IHttpActionResult GetProperty(int id)
        {
            loggedInUser = GetLoggedInUser();

            Entity.Models.Property propertyEntity = db.Properties.Find(id);
            if (propertyEntity == null)
            {
                return NotFound();
            }

            if (propertyEntity.CompanyId != loggedInUser.CompanyId)
            {
                return BadRequest("Requested property does not belong to same company as logged in user");
            }

            // Fortunately, our we have a relationship be user and associated properties, so this is easy.
            var propertyDto = Mapper.Map<Entity.Models.Property, Dto.Models.Property>(propertyEntity);
            GenerateUserPhotoLinks(propertyDto.Users);

            var result = new PropertyValidator().Validate(propertyDto);
            if (!result.IsValid)
            {
                return new ValidatorError("Error mapping property DTO from database", HttpStatusCode.InternalServerError, result, Request);
            }    

            return Ok(propertyDto);
        }
      
        // PUT: api/Properties/5
        [HttpPut]
        [Route("{id:int}", Name = "UpdatePropertyRoute")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateProperty(int id, Dto.Models.Property propertyDto)
        {
            loggedInUser = GetLoggedInUser();
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var result = new PropertyValidator().Validate(propertyDto);
            if (!result.IsValid)
            {
                return new ValidatorError("Validation failed for updated property DTO", HttpStatusCode.BadRequest, result, Request);
            }

            if (id != propertyDto.Id)
            {
                return new BadRequestErrorMessageResult("Updated property DTO id mismatch", this);
            }

            if (propertyDto.CompanyId != loggedInUser.CompanyId)
            {
                return BadRequest("Property request does not belong to same company as logged in user");
            }

            var propertyEntity = Mapper.Map<Dto.Models.Property, Entity.Models.Property>(propertyDto);
            db.Properties.Attach(propertyEntity);
            
            db.Entry(propertyEntity).State = EntityState.Modified;

            if (propertyDto.Users != null)
            {
                // Update Users for Property
                db.Entry(propertyEntity).Collection(u => u.Users).Load(); // force load
                var userIdList = propertyDto.Users.Select(u => u.Id);
                var newUsers = db.Users.Where(u => userIdList.Contains(u.Id)).ToList();
                propertyEntity.Users = newUsers; // for this to work, existing Users must have been forced loaded.            
            }            
          
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
        public IHttpActionResult NewProperty(Dto.Models.Property propertyDto)
        {
            loggedInUser = GetLoggedInUser();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new PropertyValidator().Validate(propertyDto);
            if (!result.IsValid)
            {
                return new ValidatorError("Validation failed for new property DTO", HttpStatusCode.BadRequest, result, Request);
            }

            if (propertyDto.CompanyId != loggedInUser.CompanyId)
            {
                return BadRequest("Property request does not belong to same company as logged in user");
            }

            var propertyEntity = Mapper.Map<Dto.Models.Property, Entity.Models.Property>(propertyDto);

            if (propertyDto.Users != null)
            {
                var userIdList = propertyDto.Users.Select(u => u.Id);
                var newUsers = db.Users.Where(u => userIdList.Contains(u.Id)).ToList();
                propertyEntity.Users = newUsers;                
            }            

            var company = db.Companies.Find(propertyEntity.CompanyId);
            company.Properties.Add(propertyEntity);
          
            db.SaveChanges();

            propertyDto = Mapper.Map<Entity.Models.Property, Dto.Models.Property>(propertyEntity);
            GenerateUserPhotoLinks(propertyDto.Users);
            result = new PropertyValidator().Validate(propertyDto);

            if (!result.IsValid)
            {
                return new ValidatorError("Error mapping property DTO from database", HttpStatusCode.InternalServerError, result, Request);
            }

            return CreatedAtRoute("NewPropertyRoute", new { id = propertyDto.Id }, propertyDto);            
        }

        // DELETE: api/Properties/5
        [HttpDelete]
        [Route("{id:int}", Name = "DeletePropertyRoute")]
        [ResponseType(typeof(Dto.Models.Property))]
        public IHttpActionResult DeleteProperty(int id)
        {
            loggedInUser = GetLoggedInUser();

            var propertyEntity = db.Properties.Find(id);
            if (propertyEntity == null)
            {
                return NotFound();
            }

            if (propertyEntity.CompanyId != loggedInUser.CompanyId)
            {
                return BadRequest("Requested property does not belong to same company as logged in user");
            }
           
            // Get DTO object before deleting or this will fail.
            var propertyDto = Mapper.Map<Entity.Models.Property, Dto.Models.Property>(propertyEntity);
            GenerateUserPhotoLinks(propertyDto.Users);
            var result = new PropertyValidator().Validate(propertyDto);
            if (!result.IsValid)
            {
                return new ValidatorError("Error mapping property DTO from database", HttpStatusCode.InternalServerError, result, Request);
            }

            // EF diagram won't support cascade deletes on many-to-many relationships, so we have to manually
            // delete properties for user here.
            foreach (var u in propertyEntity.Users)
            {
                u.Properties.Remove(propertyEntity);
            }

            db.Properties.Remove(propertyEntity);
         
            db.SaveChanges();

            return Ok(propertyDto);
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