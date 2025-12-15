using BookARoom.Interfaces;
using BookARoom.Models;
using BookARoom.Data;
using Microsoft.EntityFrameworkCore;

namespace BookARoom.Repository
{
    public class RoomRepo : IRoomRepo
    {
        private readonly BookingDbContext _context;
        public RoomRepo(BookingDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
    }
}
