
using System.Threading.Tasks;
using PropertyTracker.Dto.Models;


namespace PropertyTracker.Core.Services
{
    public interface IPropertyTrackerService
    {       
        // Login API
        bool LoggedIn { get; }
        Task<LoginResponse> Login(string username, string password);

        // Users API
        Task<UserList> GetUsers();
        Task<User> GetUser(int id);
        Task<User> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<User> DeleteUser(int id);

        // Properties API

    }
}