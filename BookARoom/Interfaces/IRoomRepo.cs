using BookARoom.Models;

namespace BookARoom.Interfaces
{
    public interface IRoomRepo
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
    }
}
