using System;
using System.Collections;
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
        //Fake Repository for testing
        private class FakeBookingRepo : IBookingRepo
        {
            public List<Booking> BookingsPosted { get; } = new List<Booking>();

            public Func<Booking, Task<bool>> OverlapFunc { get; set; } =
                (b) => Task.FromResult(false);
            public Func<int, Task<Booking?>> DeleteFunc { get; set; } =
                (id) => Task.FromResult<Booking?>(null);

            public Task<Booking> PostBooking(Booking booking)
            {
                BookingsPosted.Add(booking);
                if (booking.BookingId == 0)
                {
                    booking.BookingId = BookingsPosted.Count;
                }
                return Task.FromResult(booking);
            }

            public Task<IEnumerable<Booking>> GetAllBookingsRoom(int roomId, DateTime start, DateTime end)
            {
                return Task.FromResult<IEnumerable<Booking>>(BookingsPosted.Where(b =>
                    b.RoomId == roomId &&
                    b.StartTime < end &&
                    b.EndTime > start));
            }

            public Task<bool> IsOverlapAsync(Booking booking)
            {
                return OverlapFunc(booking);
            }

            public Task<IEnumerable<Booking>> GetAllActiveBookings(int roomId, DateTime currentTime)
            {
                throw new NotImplementedException();
            }

            public Task<Booking?> DeleteBooking(int bookingId)
            {
                var removeBooking = BookingsPosted.FirstOrDefault(b => b.BookingId == bookingId);
                if (removeBooking != null)
                {
                    BookingsPosted.Remove(removeBooking);
                }
                return DeleteFunc(bookingId);
            }
        }
        //Fake User Service for testing to keep user safe
        private class FakeUserService : IUserService
        {
            public string GetCurrentUser()
            {
                return "fake.user@bookaroom.com";
            }
        }

        //Here goes fact methods
        [Fact]
        public async Task CreateBooking_ShouldReturnBooking()
        {
            //Arrange
            var fakeRepo = new FakeBookingRepo();
            var fakeUserService = new FakeUserService();
            var service = new BookingService(fakeRepo, fakeUserService);
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
            Assert.Equal(fakeUserService.GetCurrentUser(), result.UserName);
        }

        [Fact]
        public async Task CreateBooking_Overlap_ShouldReturnNull()
        {
            //Arrange
            var fakeRepo = new FakeBookingRepo();
            var fakeUserService = new FakeUserService();
            var service = new BookingService(fakeRepo, fakeUserService);

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

        [Fact]
        public async Task IsAvailable_ShouldReturnFalse()
        {
            //Arrange
            var fakeRepo = new FakeBookingRepo();
            var fakeUserService = new FakeUserService();
            var service = new BookingService(fakeRepo, fakeUserService);

            var occupied = new Booking
            {
                RoomId = 101,
                UserName = "Alice",
                StartTime = new DateTime(2025, 12, 30, 10, 0, 0),
                EndTime = new DateTime(2025, 12, 30, 12, 0, 0)
            };
            fakeRepo.BookingsPosted.Add(occupied);

            var searchRoom = new Room { 
                RoomId = 101,
                RoomName = "Conference Room"
            };
            var searchStart = new DateTime(2025, 12, 30, 10, 30, 0);
            var searchEnd = new DateTime(2025, 12, 30, 12, 30, 0);

            //Act
            var isAvailable = await service.IsAvailable(searchRoom.RoomId, searchStart, searchEnd);

            //Assert
           Assert.False(isAvailable);
        }

        [Fact]
        public async Task DeleteBooking_ShouldCallRepoAndReturnTrue()
        {
            //Arrange
            var fakeRepo = new FakeBookingRepo();
            var fakeUserService = new FakeUserService();
            var service = new BookingService(fakeRepo, fakeUserService);
            var bookingToDelete = new Booking
            {
                BookingId = 1,
                RoomId = 1,
                UserName = "Bob",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1)
            };
            fakeRepo.BookingsPosted.Add(bookingToDelete);
            fakeRepo.DeleteFunc = (id) => Task.FromResult(bookingToDelete);

            //Act
            var result = await service.DeleteBooking(bookingToDelete.BookingId);

            //Assert
            Assert.True(result);
            Assert.Empty(fakeRepo.BookingsPosted);
        }
    }
}
