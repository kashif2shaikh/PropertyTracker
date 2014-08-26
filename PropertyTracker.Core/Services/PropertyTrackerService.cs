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
    public class PropertyTrackerService : IPropertyTrackerService
    {
        // TODO: This should move into some configurable project/build setting
        private const string PropertyTrackerServer = "http://192.168.15.60";
        private const string PropertyTrackerBaseAddress = PropertyTrackerServer + "/PropertyTracker.Web.Api/api/";

        private const string LoginRequestUrl = "login";

        private HttpClient _client;
        private HttpClientHandler _handler;
        public bool LoggedIn { get; private set; }

       

        public PropertyTrackerService()
        {
            
            LoggedIn = false;
            _handler = new HttpClientHandler();                       
            _client = new HttpClient(_handler)
            {                
                BaseAddress = new Uri(PropertyTrackerBaseAddress),
				Timeout = TimeSpan.FromSeconds(10),
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       

            /* HttpClient must be initialized on login since */

        }

        public async Task<LoginResponse> Login(string username, string password)
		{
			using (var handler = new HttpClientHandler ())
			using (var client = new HttpClient (handler)) {
				handler.Credentials = new NetworkCredential (username, password);
				//client.BaseAddress = new Uri (PropertyTrackerBaseAddress);
				client.DefaultRequestHeaders.Accept.Clear ();
				client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));


				//_handler.Credentials = new NetworkCredential(username, password);

				//
				// Yes, I know - we are duplicating credentials within payload - in future if we want to get away from basic auth, we can do so here.
				// 
				// #future
				//
				var loginRequest = new LoginRequest {
					Username = username,
					Password = password
				};

				// #reference: For async style: var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload));
				var jsonContent = new StringContent (JsonConvert.SerializeObject (loginRequest), Encoding.UTF8, "application/json");

				//var uri = new Uri()
				HttpResponseMessage response = await client.GetAsync ("http://192.168.15.60/PropertyTracker.Web.Api/api/users/1");
				if (response.IsSuccessStatusCode == false) {
					//Console.WriteLine("Request failed: " + response.ToString());
					// #todo we need some kind of logging to print out result
					return null;
				}

				var responseContent = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<LoginResponse> (responseContent);
			}
		}

    }
}
