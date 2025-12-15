using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookARoom.Models;

namespace BookARoom.Interfaces
{
    public interface IBookingService
    {
        Task<Booking?> CreateBooking(Booking booking);
        Task<bool> IsAvailable(int roomId, DateTime startTime, DateTime endTime); 
        Task<bool> DeleteBooking(int bookingId);
    }
}
