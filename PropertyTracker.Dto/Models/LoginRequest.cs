namespace PropertyTracker.Dto.Models
{
    // This class is largely redundant, since when User Logs in, it will pass username/password within Basic Authorization Header
    // But if we want to pass more data across, or in future, move away from Basic scheme, this class will make it very easy to do.
    public class LoginRequest
    {
        public LoginRequest()
        {
            
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
