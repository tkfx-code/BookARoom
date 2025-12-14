using BookARoom.Interfaces;
using BookARoom.Models;
using BookARoom.Data;
using Microsoft.EntityFrameworkCore;

namespace BookARoom.Repository
{
    public class BookingRepo : IBookingRepo
    {
        private readonly BookingDbContext _context;

        public BookingRepo(BookingDbContext context)
        {
            _context = context;
        }

        public Task<Booking?> CreateBooking(Booking booking)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> IsOverlapAsync(Booking booking)
        {
            var overlapBooking = await GetAllBookingsRoom(
                booking.RoomId,
                booking.StartTime,
                booking.EndTime
            );
            return overlapBooking.Any();
        }
        public async Task<Booking> PostBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsRoom(int roomId, DateTime startTime, DateTime endTime)
        {
            var bookings = await _context.Bookings
                .Where(b =>
                b.RoomId == roomId &&
                b.StartTime < endTime &&
                b.EndTime > startTime)
                .ToListAsync();

            return bookings;
        }
    }
}
