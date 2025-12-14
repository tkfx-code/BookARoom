using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookARoom.Models;

namespace BookARoom.Interfaces
{
    public interface IBookingRepo
    {
        public Task<IEnumerable<Booking>> GetAllBookingsRoom(int roomId, DateTime startTime, DateTime endTime);
        public Task<bool> IsOverlapAsync(Booking booking);
        public Task<Booking> PostBooking(Booking booking);
        public Task<IEnumerable<Booking>> GetAllActiveBookings(int roomId, DateTime currentTime);
    }
}
