using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
    public class UsersController : BaseApiController
    {
        private PropertyTrackerContext db = new PropertyTrackerContext();
        private Entity.Models.User loggedInUser = null;

        // GET: api/Users
        [HttpGet]
        [Route("", Name="GetUserListRoute")]        
        [ResponseType(typeof(Dto.Models.UserList))]
        public IHttpActionResult GetUsers()
        {
            loggedInUser = GetLoggedInUser();

            var entityUserList = db.Users.Where(u => u.CompanyId == loggedInUser.CompanyId);

            var userDtoList = Mapper.Map<IEnumerable<Entity.Models.User>, Dto.Models.UserList>(entityUserList);
            GenerateUserPhotoLinks(userDtoList);
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
            loggedInUser = GetLoggedInUser();

            Entity.Models.User userEntity = db.Users.FirstOrDefault(u => u.CompanyId == loggedInUser.CompanyId && u.Id == id);
            if (userEntity == null)
            {
                return NotFound();
            }

            var userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            GenerateUserPhotoLink(userDto);
            ValidationResult userValidatorResult = new UserValidator().Validate(userDto, ruleSet: "default,NoPassword");
            
            if (!userValidatorResult.IsValid)
            {
                return new ValidatorError("Error mapping user DTO from database", HttpStatusCode.InternalServerError, userValidatorResult, Request);
                // I can also do something like this: 
                //return ResponseMessage( new ValidatorError("Error mapping user DTO from database", HttpStatusCode.InternalServerError, userValidatorResult, Request).Response);
            }


            return Ok(userDto);
        }

        // GET: api/Users/5/photo
        [HttpGet]
        [AllowAnonymous]
        [Route("{id:int}/photo", Name = "GetUserPhotoRoute")]        
        public async Task<HttpResponseMessage> GetUserPhoto(int id)
        {            
            var cancelToken = new CancellationToken();

            Entity.Models.User userEntity = await db.Users.FindAsync(cancelToken, id);
            if (userEntity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var photoData = userEntity.Photo;
            if (photoData == null)
            {
                // TODO - learn how to serve static files, but this works for now
                //var root = HttpContext.Current.Server.MapPath("~/App_Data");
                //photoData = File.ReadAllBytes(root + "/nouser@2x.png");

                // get random photo
                using (var client = new HttpClient())
                using (var imageResponse = await client.GetAsync("http://lorempixel.com/256/256/people/" + userEntity.Username, cancelToken))
                {
                    if (imageResponse.IsSuccessStatusCode)
                    {
                        photoData = await imageResponse.Content.ReadAsByteArrayAsync();
                    }                        
                }                                    
            }

            //Image img = (System.Drawing.Image) userEntity.Photo;
            //byte[] imgData = img.ImageData;
            //MemoryStream ms = new MemoryStream(userEntity.Photo);
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(photoData),              
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            return response;            
        }

        // Post: api/Users/5/photo
        [HttpPost]
        [Route("{id:int}/photo", Name = "UploadUserPhotoRoute")]

        [ResponseType(typeof (void))]
        public async Task<HttpResponseMessage> UploadUserPhoto(int id)
        {
            loggedInUser = GetLoggedInUser();

            Entity.Models.User userEntity = db.Users.FirstOrDefault(u => u.CompanyId == loggedInUser.CompanyId && u.Id == id);
            if (userEntity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
       
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);

                    // TODO - should be async read/write
                    var info = new FileInfo(file.LocalFileName);
                    userEntity.Photo = File.ReadAllBytes(info.FullName);
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }            
        }

        // PUT: api/Users/5
        [HttpPut] 
        [Route("{id:int}", Name = "UpdateUserRoute")]
        [ResponseType(typeof(void))]        
        public IHttpActionResult UpdateUser(int id, Dto.Models.User userDto)
        {
            loggedInUser = GetLoggedInUser();

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
            if (userEntity.CompanyId != loggedInUser.CompanyId)
            {
                // Updated user does not have same company. Make it appear as user does not exist for this company.
                return NotFound();
            }
            else if (db.Users.Count(u => u.CompanyId == loggedInUser.CompanyId && u.Id != userEntity.Id && u.Username == userEntity.Username) > 0)
            {
                return new BadRequestErrorMessageResult("Another user has the same username as this user", this);
            }

            db.Users.Attach(userEntity);
            // Don't mark entire entity as modified - fields are optional
            //db.Entry(userEntity).State = EntityState.Modified;

            if (userEntity.Fullname != null)
            {
                db.Entry(userEntity).Property(u => u.Fullname).IsModified = true;
            }

            if (userEntity.Username != null)
            {
                db.Entry(userEntity).Property(u => u.Username).IsModified = true;
            }

            if (userEntity.Password != null)
            {
                db.Entry(userEntity).Property(u => u.Password).IsModified = true;
            }
           
            if (userEntity.Password == null)
            {
                // Entity validation will fail because Password column is not-null and password is optional field.
                // NOTE: Must use Where/Select instead of Find, so entire entity is not loaded (otherwise it will conflict with Attach!)
                //userEntity.Password = db.Users.Where(u => u.Id == userEntity.Id).Select(u => u.Password).FirstOrDefault();
            }

            if (userDto.Properties != null)
            {
                db.Entry(userEntity).Collection(u => u.Properties).Load(); // force load         
                var propertyIdList = userDto.Properties.Select(p => p.Id);
                var newProperties = db.Properties.Where(p => propertyIdList.Contains(p.Id)).ToList();
                
                userEntity.Properties = newProperties; // for this to work you must force load existing Property collection
            }


           
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;
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
            finally
            {
                db.Configuration.ValidateOnSaveEnabled = true;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [HttpPost]
        [Route("", Name = "NewUserRoute")]
        [ResponseType(typeof(Dto.Models.User))]
        public IHttpActionResult NewUser(Dto.Models.User userDto)
        {
            loggedInUser = GetLoggedInUser();

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
            if (userEntity.CompanyId != loggedInUser.CompanyId)
            {
                return new BadRequestErrorMessageResult("New user does not have same company as logged in user", this);
            }
            else if (db.Users.Count(u => u.CompanyId == loggedInUser.CompanyId && u.Id != userEntity.Id && u.Username == userEntity.Username) > 0)
            {
                return new BadRequestErrorMessageResult("Another user has the same username as this user", this);
            }

            if (userDto.Properties != null)
            {
                var propertyIdList = userDto.Properties.Select(p => p.Id);
                var properties = db.Properties.Where(p => propertyIdList.Contains(p.Id));
                foreach (var p in properties)
                {
                    p.Users.Add(userEntity);
                }
            }


            var company = db.Companies.Find(userDto.Company.Id);
            company.Users.Add(userEntity);

         
            
            db.SaveChanges();
            
            userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            GenerateUserPhotoLink(userDto);
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
            loggedInUser = GetLoggedInUser();

            Entity.Models.User userEntity = db.Users.FirstOrDefault(u => u.CompanyId == loggedInUser.CompanyId && u.Id == id);
            if (userEntity == null)
            {
                return NotFound();
            }

            // Get DTO object before deleting or this will fail.
            var userDto = Mapper.Map<Entity.Models.User, Dto.Models.User>(userEntity);
            GenerateUserPhotoLink(userDto);
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