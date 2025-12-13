using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookARoom.Interfaces;
using BookARoom.Models;
using BookARoom.Services;

namespace Test.BookARoom
{
    public class BookingServiceTest
    {
        private class FakeBookingRepo : IBookingRepo
        {
            public List<Booking> BookingsPosted { get; } = new List<Booking>();
            public Func<Booking, Task<bool>> OverlapFunc { get; set; } =
                (b) => Task.FromResult(false);
            public Task<Booking> PostBooking(Booking booking)
            {
                BookingsPosted.Add(booking);
                if (booking.BookingId == 0)
                {
                    booking.BookingId = BookingsPosted.Count;
                }
                return Task.FromResult(booking);
            }

            public Task<bool> IsOverlapAsync(Booking booking)
            {
                return OverlapFunc(booking);
            }

            public Task<Booking?> CreateBooking(Booking booking)
            {
                throw new NotImplementedException();
            }
        }

        public Task<bool> IsOverlapAsync(Booking booking)
        {
            return Task.FromResult(false);
        }
        //more methods from IBookingRepo when needed

        //Here goes fact methods
        [Fact]
        public async Task CreateBooking_ShouldReturnBooking()
        {
            //Arrange
            var fakeRepo = new FakeBookingRepo();
            var service = new BookingService(fakeRepo);
            var newBooking = new Booking
            {
                RoomId = 1,
                UserName = "Test",
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1)
            };

            //Act
            var result = await service.CreateBooking(newBooking);

            //Assert
            Assert.Single(fakeRepo.BookingsPosted);
            Assert.NotNull(result);
            Assert.Equal(newBooking.RoomId, result.RoomId);
        }

        [Fact]
        public async Task CreateBooking_Overlap_ShouldReturnNull()
        {
            //Arramge
            var fakeRepo = new FakeBookingRepo();
            var service = new BookingService(fakeRepo);

            fakeRepo.OverlapFunc = (b) => Task.FromResult(true);

            var newBooking = new Booking
            {
                RoomId = 1,
                StartTime = DateTime.Now.AddDays(1)
            };

            //Act
            var result = await service.CreateBooking(newBooking);

            //Assert
            Assert.Null(result);
            Assert.Empty(fakeRepo.BookingsPosted);
        }
    }
}
