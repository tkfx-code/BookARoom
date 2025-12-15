using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookARoom.Data;
using BookARoom.Models;
using BookARoom.Repository;
using BookARoom.Interfaces;

namespace BookingWebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService bookingService;
        private readonly IRoomRepo roomRepo;

        public BookingsController(IRoomRepo roomRepo, IBookingService bookingService)
        {
            this.roomRepo = roomRepo;
            this.bookingService = bookingService;
        }

        //GET: api/Bookings/rooms
        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await roomRepo.GetAllRoomsAsync();
            return Ok(rooms);
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBodyAttribute]Booking booking)
        {
            if (booking.StartTime >= booking.EndTime)
            {
                return BadRequest("End time must be after start time.");
            }

            var newBooking = await bookingService.CreateBooking(booking);

            if (newBooking == null)
            {
                return Conflict("The booking overlaps with an existing booking.");
            }

            //Return 201 at created
            return CreatedAtAction(nameof(GetRooms), new { id = newBooking.BookingId }, newBooking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var success = await bookingService.DeleteBooking(id);
            if (!success)
            {
                return NotFound("Could not find booking with matching ID");
            }

            return NoContent();
        }

        //private bool BookingExists(int id)
        //{
        //    return _context.Bookings.Any(e => e.BookingId == id);
        //}

        //// GET: api/Bookings
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        //{
        //    return await _context.Bookings.ToListAsync();
        //}

        //// GET: api/Bookings/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Booking>> GetBooking(int id)
        //{
        //    var booking = await _context.Bookings.FindAsync(id);

        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    return booking;
        //}

        //// PUT: api/Bookings/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBooking(int id, Booking booking)
        //{
        //    if (id != booking.BookingId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(booking).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BookingExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}




    }
}
