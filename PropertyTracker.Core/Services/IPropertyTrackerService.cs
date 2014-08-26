
using System.Threading.Tasks;
using PropertyTracker.Dto.Models;


namespace PropertyTracker.Core.Services
{
    public interface IPropertyTrackerService
    {
        bool LoggedIn { get; }

        Task<LoginResponse> Login(string username, string password);
    }
}