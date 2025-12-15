using BookARoom.Models;
using BookARoom.Interfaces;
using BookARoom.Repository;
using System;


namespace BookARoom.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IUserService _userService;

        public BookingService(IBookingRepo bookingRepo, IUserService userService)
        {
            _bookingRepo = bookingRepo;
            _userService = userService;
        }

        public async Task<Booking?> CreateBooking(Booking booking)
        {
            //GetUserId currently hard coded for mocked testing
            booking.UserName = _userService.GetUserId();

            bool isOverlapping = await _bookingRepo.IsOverlapAsync(booking);

            if (isOverlapping)
            {
                return null;
            }
            var savedBooking = await _bookingRepo.PostBooking(booking);

            return savedBooking;
        }

        public async Task<bool> IsAvailable(int roomId, DateTime startTime, DateTime endTime)
        {
            var overlapBooking = await _bookingRepo.GetAllBookingsRoom(
                roomId,
                startTime,
                endTime
            );
            return !overlapBooking.Any();
        }
    }
}
