
using System.Threading.Tasks;
using PropertyTracker.Dto.Models;


namespace PropertyTracker.Core.Services
{
    public interface IPropertyTrackerService
    {       
        // Login API
        bool LoggedIn { get; }
        User LoggedInUser { get; }
        Task<object> Login(string username, string password);
        void Logout();

        // Users API
        Task<object> GetUsers();
        Task<object> GetUser(int id);
        Task<object> AddUser(User user);
        Task<object> UpdateUser(User user);
        Task<object> DeleteUser(int id);
		Task<object> UploadUserPhoto (int id, byte[] photoData);
		Task<object> DownloadUserPhoto (User user);

        // Properties API
        Task<object> GetProperties(PropertyListRequest requestParams);

    }
}