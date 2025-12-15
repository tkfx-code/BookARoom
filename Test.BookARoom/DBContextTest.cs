using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookARoom.Data;
using BookARoom.Models;
using BookARoom.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.BookARoom
{
    public class DBContextTest
    {
        private BookingDbContext RealDbContext ()
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Test.BookARoom;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new BookingDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            
            return dbContext;
        }
        [Fact]
        public async Task PostBooking_RealDB_ShouldSaveAndFetch()
        {
            //Arrange
            using var context = RealDbContext();
            var repo = new BookingRepo(context);

            //Data Seed
            var room = new Room
            {
                RoomName = "Integration Test Room",
            };
            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            var expected = new Booking
            {
                RoomId = room.RoomId,
                UserName = "Integration tester",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2)
            };

            //Act
            await repo.PostBooking(expected);

            //Assert
            var actualBooking = await context.Bookings.FindAsync(expected.BookingId);
            Assert.NotNull(actualBooking);
            Assert.Equal(expected.RoomId, actualBooking.RoomId);
            Assert.Equal(expected.UserName, actualBooking.UserName);
        }
    }
}
