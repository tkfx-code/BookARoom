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
            var repo = new BookingServiceRepo(context);

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
    }
}
