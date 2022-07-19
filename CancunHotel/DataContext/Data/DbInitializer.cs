using CancunHotel.Models;

using Microsoft.EntityFrameworkCore;


namespace CancunHotel.DataContext.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var _context = new WebApiDbContext(serviceProvider.GetRequiredService<DbContextOptions<WebApiDbContext>>()))
            {
                if(_context.Rooms.Any()) { return; }

                _context.Rooms.AddRange(
                    new Room { RoomNumber = 101 }
                );

                if (_context.Guests.Any()) { return; }

                if (_context.Bookings.Any()) { return; }

            }
        }
    }
}
