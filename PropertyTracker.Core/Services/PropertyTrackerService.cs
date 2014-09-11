using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;
using PropertyTracker.Dto.Models;
using System.Globalization;
using System.IO;

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
        private const string UsersRequestUrl = "users";
        private const string PropertiesRequestUrl = "properties";
		private const string PhotoUrl = "photo";

        private readonly HttpClient _client;
        private readonly HttpClientHandler _handler;
        
        public bool LoggedIn { get; private set; }

        public User LoggedInUser { get; private set; }

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

        public async Task<object> Login(string username, string password)
        {
            LoggedIn = false;
            LoggedInUser = null;

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
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                if (response.IsSuccessStatusCode == false)
                {
                    var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;
                    Debug.WriteLine("Request failed: " + response.ToString());
                    _client.DefaultRequestHeaders.Authorization = null;
                    return errorResult;
                }
                
                var parsedObject = JsonConvert.DeserializeObject<LoginResponse>(content);
                if (parsedObject != null)
                {
                    LoggedIn = true;
                    LoggedInUser = parsedObject.User;
                }
                else
                {
                    Debug.WriteLine("Could not deserialize json(" + content + ") to login response");                  
                }
                LoggedIn = parsedObject != null;
                return parsedObject;
            }
        }

        public async void Logout()
        {
            LoggedIn = false;
            LoggedInUser = null;
        }

        public async Task<object> GetUsers()
        {
            if (!LoggedIn)
            {
                Debug.WriteLine("Not logged in");
                return null;
            }
            using (var response = await _client.GetAsync(UsersRequestUrl))
            {
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                if (response.IsSuccessStatusCode == false)
                {
                    var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;
                    Debug.WriteLine("Request failed: " + response.ToString());
                    return errorResult;
                }

                var parsedObject = JsonConvert.DeserializeObject<UserList>(content);
                if (parsedObject == null)
                {
                    Debug.WriteLine("Could not deserialize json(" + content + ") to userlist response");                    
                }
                return parsedObject;
            }
        }

        public async Task<object> GetUser(int id)
        {
            if (!LoggedIn)
            {
                Debug.WriteLine("Not logged in");
                return null;
            }
            using (var response = await _client.GetAsync(string.Format("{0}/{1}", UsersRequestUrl, id)))            
            {
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                if (response.IsSuccessStatusCode == false)
                {
                    var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;
                    Debug.WriteLine("Request failed: " + response.ToString());
                    return errorResult;
                }                
                var parsedObject = JsonConvert.DeserializeObject<User>(content);
                if (parsedObject == null)
                {
                    Debug.WriteLine("Could not deserialize json(" + content + ") to user response");
                }
                return parsedObject;
            }
        }

        public async Task<object> AddUser(User user)
        {
            if (!LoggedIn)
            {
                Debug.WriteLine("Not logged in");
                return null;
            }
            var payload = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            using (var response = await _client.PostAsync(UsersRequestUrl, payload))
            {
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                if (response.IsSuccessStatusCode == false)
                {
                    var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;
                    Debug.WriteLine("Request failed: " + response.ToString());
                    return errorResult;
                }                
                var parsedObject = JsonConvert.DeserializeObject<User>(content);
                if (parsedObject == null)
                {
                    Debug.WriteLine("Could not deserialize json(" + content + ") to user response");
                }
                return parsedObject;
            }
        }

        public async Task<object> UpdateUser(User user)
        {
            if (!LoggedIn)
            {
                Debug.WriteLine("Not logged in");
                return null;
            }
            var payload = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            using (var response = await _client.PutAsync(string.Format("{0}/{1}", UsersRequestUrl, user.Id),payload))
            {
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                if (response.IsSuccessStatusCode == false)
                {
                    var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;
                    Debug.WriteLine("Request failed: " + response.ToString());
                    return errorResult;
                }
                return true;
            }
        }


        public async Task<object> DeleteUser(int id)
        {
            if (!LoggedIn)
            {
                Debug.WriteLine("Not logged in");
                return null;
            }
            using (var response = await _client.DeleteAsync(string.Format("{0}/{1}", UsersRequestUrl, id)))
            {
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                if (response.IsSuccessStatusCode == false)
                {
                    var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;
                    Debug.WriteLine("Request failed: " + response.ToString());
                    return errorResult;
                }
                
                var parsedObject = JsonConvert.DeserializeObject<User>(content);
                if (parsedObject == null)
                {                    
                    Debug.WriteLine("Could not deserialize json(" + content + ")");
                }
                return parsedObject;
            }
        }

		public async Task<object> UploadUserPhoto (int id, byte[] photoData)
		{
			if (!LoggedIn)
			{
				Debug.WriteLine("Not logged in");
				return null;
			}
			using(var requestContent = new MultipartFormDataContent())
			{
				//    here you can specify boundary if you need---^
				var imageContent = new ByteArrayContent(photoData);
				imageContent.Headers.ContentType = 
					MediaTypeHeaderValue.Parse("image/jpeg");

				requestContent.Add(imageContent, "image", "image.jpg");

				using (var response = await _client.PostAsync(string.Format("{0}/{1}/{2}", UsersRequestUrl, id, PhotoUrl), requestContent))
				{
					var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
					if (response.IsSuccessStatusCode == false)
					{
						var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;
						Debug.WriteLine("Request failed: " + response.ToString());
						return errorResult;
					}

					return true;
				}

			}

		}

        public async Task<object> GetProperties(PropertyListRequest requestParams)
        {
            if (!LoggedIn)
            {
                Debug.WriteLine("Not logged in");
                return null;
            }

            var url = PropertiesRequestUrl.SetQueryParams(requestParams);

			// Flurl seems to skip bool parameters. Add it manually.
			url.SetQueryParam ("SortAscending", requestParams.SortAscending);

            using (var response = await _client.GetAsync(url))            
            {
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                if (response.IsSuccessStatusCode == false)
                {
                    var errorResult = content != null ? JsonConvert.DeserializeObject<ErrorResult>(content) : null;                    
                    Debug.WriteLine("Request failed: " + response.ToString());
                    return errorResult;
                }
                
                var parsedObject = JsonConvert.DeserializeObject<PaginatedPropertyList>(content);
                if (parsedObject == null)
                {
                   
                        Debug.WriteLine("Could not deserialize json(" + content + ")");
                }
                return parsedObject;
            }
           
        }
    }
}
