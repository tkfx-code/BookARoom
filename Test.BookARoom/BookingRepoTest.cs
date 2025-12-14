using BookARoom.Data;
using BookARoom.Models;
using BookARoom.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.BookARoom
{
    public class BookingRepoTest
    {
        private BookingDbContext InMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new BookingDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task SaveBooking_ShouldStoreBooking()
        {
            //Arrange
            using var context = InMemoryDbContext();
            var repo = new BookingRepo(context);

            //Act
            var newBooking = new Booking 
            { 
                RoomId = 1, 
                UserName = "TestUser",
                StartTime = DateTime.Now, 
                EndTime = DateTime.Now.AddHours(1) 
            };
            await repo.PostBooking(newBooking);

            //Assert
            var savedBooking = await context.Bookings.FindAsync(newBooking.BookingId);
            Assert.NotNull(savedBooking);
            Assert.Equal(1, savedBooking.RoomId);
        }

        [Fact]
        public async Task GetAllBookingsRoom_ShouldFindOverlap()
        {
            //Arrange
            using var context = InMemoryDbContext();
            var repo = new BookingRepo(context);

            var occupied = new Booking
            {
                RoomId = 101,
                UserName = "First User",
                StartTime = new DateTime(2025, 1, 1, 10, 0, 0),
                EndTime = new DateTime(2025, 1, 1, 11, 0, 0)
            };

            var nonOverlap = new Booking
            {
                RoomId = 101,
                UserName = "Next User",
                StartTime = new DateTime(2025, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2025, 1, 1, 13, 0, 0)
            };

            context.Bookings.Add(occupied);
            context.Bookings.Add(nonOverlap);
            await context.SaveChangesAsync();

            int searchRoom = 101;
            var searchStart = new DateTime(2025, 1, 1, 10, 30, 0);
            var searchEnd = new DateTime(2025, 1, 1, 12, 30, 0);

            //Act
            var overlappingBooking = await repo.GetAllBookingsRoom(searchRoom, searchStart, searchEnd);

            //Assert 
            Assert.Equal(2, overlappingBooking.Count());
            Assert.Equal("First User", overlappingBooking.First().UserName);
        }
    }
}
