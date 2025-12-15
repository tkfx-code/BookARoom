using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookARoom.Models;

namespace BookARoom.Data
{
    public class BookingDbContext : DbContext
    {

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
            
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne<Room>()
                .WithMany()
                .HasForeignKey(b => b.RoomId);
        }
    }
}
