

using BookARoom.Interfaces;

namespace BookARoom.Services
{
    public class UserService : IUserService
    {
        public string GetCurrentUser()
        {
            // TestRun UserID, in real scenario, get ID from auth user context
            return "fake.user@bookaroom.com";
        }
    }
}
