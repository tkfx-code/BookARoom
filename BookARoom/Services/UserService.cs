

using BookARoom.Interfaces;

namespace BookARoom.Services
{
    public class UserService : IUserService
    {
        public string GetUserId()
        {
            // TestRun UserID, in real scenario, get ID from auth user context
            return "sample-user-id";
        }
    }
}
