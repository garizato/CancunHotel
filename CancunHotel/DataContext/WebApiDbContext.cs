using CancunHotel.Models;
using Microsoft.EntityFrameworkCore;

namespace CancunHotel.DataContext
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext() { }

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Guest> Guests { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase("CancunHotelDB");
        }
    }
}
