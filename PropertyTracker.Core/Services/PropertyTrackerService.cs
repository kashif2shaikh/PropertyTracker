using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyTracker.Dto.Models;

namespace PropertyTracker.Core.Services
{
    public class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        public BasicAuthenticationHeaderValue(string username, string password) : 
            base("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)))
        {
            
        }
    }

    public class PropertyTrackerService : IPropertyTrackerService
    {
        // TODO: This should move into some configurable project/build setting
        private const string PropertyTrackerServer = "http://192.168.15.60";
        private const string PropertyTrackerBaseAddress = PropertyTrackerServer + "/PropertyTracker.Web.Api/api/";

        // These are all relative to base
        private const string LoginRequestUrl= "login";
        private const string UserRequestUrl = "users";
        private const string PropertyRequestUrl = "property";

        private readonly HttpClient _client;
        private readonly HttpClientHandler _handler;
        public bool LoggedIn { get; private set; }
      
        public PropertyTrackerService()
        {
            
            LoggedIn = false;
            _handler = new HttpClientHandler
            {
                UseProxy = false, // must disable otherwise network requests will hang when using Mac debugging proxy (e.g. Charles Proxy)
                AllowAutoRedirect = false
            };
            
            _client = new HttpClient(_handler)
            {                
                BaseAddress = new Uri(PropertyTrackerBaseAddress),
				Timeout = TimeSpan.FromSeconds(60),
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       

            /* HttpClient must be initialized on login since */

        }

        public async Task<LoginResponse> Login(string username, string password)
        {           
            // Setup Authorization header - 
            _client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(username, password);
            
            //
            // Yes, I know - we are duplicating credentials within payload - in future if we want to get away from basic auth, we can do so here.
            // 
            // #future we may want to tack in more information within login request
            //
			var loginRequest = new LoginRequest {
				Username = username,
				Password = password
			};

		    // #reference: For async style: var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload));
		    var payload = new StringContent (JsonConvert.SerializeObject (loginRequest), Encoding.UTF8, "application/json");

            using (var response = await _client.PostAsync(LoginRequestUrl, payload))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    //Console.WriteLine("Request failed: " + response.ToString());
                    // #todo we need some kind of logging to print out result
                    _client.DefaultRequestHeaders.Authorization = null;
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LoginResponse>(content);
            }
        }
    }
}
