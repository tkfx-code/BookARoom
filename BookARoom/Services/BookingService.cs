using BookARoom.Models;
using BookARoom.Interfaces;


namespace BookARoom.Services
{
    public class BookingService : IBookingRepo
    {
        private readonly IBookingRepo _bookingRepo;

        public BookingService(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        public async Task<Booking?> CreateBooking(Booking booking)
        {
            bool isOverlapping = await _bookingRepo.IsOverlapAsync(booking);

            if (isOverlapping)
            {
                return null;
            }
            var savedBooking = await _bookingRepo.PostBooking(booking);

            return savedBooking;
        }

        public Task<bool> IsOverlapAsync(Booking booking)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> PostBooking(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
