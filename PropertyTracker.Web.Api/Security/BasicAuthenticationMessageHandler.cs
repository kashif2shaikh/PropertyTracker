// BasicAuthenticationMessageHandler.cs
// Copyright Jamie Kurtz, Brian Wortman 2014.

using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using Ninject.Planning.Bindings.Resolvers;
using PropertyTracker.Web.Api.Common;
using PropertyTracker.Web.Api.Routing;
using PropertyTracker.Web.Entity.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PropertyTracker.Web.Api.Security
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {      
        public const char AuthorizationHeaderSeparator = ':';
        private const int UsernameIndex = 0;
        private const int PasswordIndex = 1;
        private const int ExpectedCredentialCount = 2;

        public BasicAuthenticationMessageHandler()
        {
            
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (AuthenticateRequest(request))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // Authentication failed - send back 401 Unauthorized
            return CreateUnauthorizedResponse();
        }

        public bool SetPrincipal(User user)
        {
            IPrincipal principal = GetPrincipal(user);
            if (principal == null)
            {
                Console.WriteLine("System could not create principal for user {0}", user.Username);
                return false;
            }

            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }

            return true;
        }

        public IPrincipal GetPrincipal(User user)
        {
            var identity = new UserIdentity(user, Constants.SchemeTypes.Basic);

            identity.AddClaim(new Claim(ClaimTypes.Name, user.Fullname));
            identity.AddClaim(new Claim(ClaimTypes.Role, Constants.RoleNames.Standard));

            return new ClaimsPrincipal(identity);
        }

        public User GetUser(string username)
        {
            // Authentication is performed for each user - so we don't 
            username = username.ToLowerInvariant();
            User user = null;            
            
            using (var db = new PropertyTrackerContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                user = db.Users.Include(u => u.Company).SingleOrDefault(u => u.Username == username);
            }
            return user;
        }

        public bool AuthenticateRequest(HttpRequestMessage request)
        {
            // NOTE: For basic authentication, the user is going to be authenticated against every request.
            // Which will result in Db lookup.
            if(IsAuthenticated())
            {
                // Already authenticated.
                return true;
            }

            if (!CanHandleAuthentication(request))
            {
                // No basic scheme - just return true. Authorization filter will handle.
                return true;
            }

            bool isAuthenticated;
            try
            {
                isAuthenticated = Authenticate(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure in auth processing", e);
                return false;
            }

            if (isAuthenticated)
            {
                return true;
            }

            return false;
        }

        private bool IsAuthenticated()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Console.WriteLine("Already authenticated");
                return true;
            }
            return false;
        }

        private bool CanHandleAuthentication(HttpRequestMessage request)
        {
            if (request.Headers != null
                && request.Headers.Authorization != null
                && request.Headers.Authorization.Scheme.ToLowerInvariant() == Constants.SchemeTypes.Basic)
            {
                return true; // valid basic scheme request
            }
            Console.WriteLine("Not a basic auth request");
            return false;            
        }

        private bool Authenticate(HttpRequestMessage request)
        {
            Console.WriteLine("Attempting to authenticate...");

            var authHeader = request.Headers.Authorization;
            if (authHeader == null)
            {
                return false;
            }

            var credentialParts = GetCredentialParts(authHeader);
            if (credentialParts.Length != ExpectedCredentialCount)
            {
                return false;
            }

            var username = credentialParts[UsernameIndex];
            var password = credentialParts[PasswordIndex];

            var user = GetUser(username);
            if (user == null)
            {
                Console.WriteLine("Can't find username in db: {0}", username);
                return false;
            }

            if (user.Password != password)
            {
                Console.WriteLine("Password does not match for username: {0}", username);
                return false;
            }


            return SetPrincipal(user);
        }

        private string[] GetCredentialParts(AuthenticationHeaderValue authHeader)
        {
            var encodedCredentials = authHeader.Parameter;
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.ASCII.GetString(credentialBytes);
            var credentialParts = credentials.Split(AuthorizationHeaderSeparator);
            return credentialParts;
        }

        private HttpResponseMessage CreateUnauthorizedResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Constants.SchemeTypes.Basic));
            return response;
        }
        
       
    }
}