using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookARoom.Interfaces;
using BookARoom.Models;

namespace BookARoom.Repository
{
    public class BookingRepo : IBookingRepo
    {
        public Task<Booking?> CreateBooking(Booking booking)
        {
            throw new NotImplementedException();
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
