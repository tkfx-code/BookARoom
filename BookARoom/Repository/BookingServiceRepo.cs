using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookARoom.Data;
using BookARoom.Interfaces;
using BookARoom.Models;

namespace BookARoom.Repository
{
    public class BookingServiceRepo : IBookingRepo
    {
        private readonly BookingDbContext _context;
        public BookingServiceRepo(BookingDbContext context) {
            _context = context;
        }

        public Task<Booking?> CreateBooking(Booking booking)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsOverlapAsync(Booking booking)
        {
            throw new NotImplementedException();
        }

        public async Task<Booking> PostBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public Task<IEnumerable<Booking>> GetAllBookingsRoom(int roomId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        //public async Task methods
        //return await context.Booking.ToListAsync
    }
}
