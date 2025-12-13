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
        Task<bool> IsOverlapAsync(Booking booking);
        Task<Booking> PostBooking(Booking booking);
        //Overlap
        //availability
        //
    }
}
