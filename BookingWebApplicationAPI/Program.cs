
using BookARoom.Data;
using BookARoom.Interfaces;
using BookARoom.Services;
using BookARoom.Repository;
using Microsoft.EntityFrameworkCore;


namespace BookingWebApplicationAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Add services to the container.
            //Make sure it points to the correct Migration assembly in BookARoom Class Library
            builder.Services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("BookARoom"))
            );

            //Add DI
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
