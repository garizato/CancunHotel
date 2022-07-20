using CancunHotel.DataContext;
using CancunHotel.Models;

namespace CancunHotel.Services
{
    public interface IGuestService
    {
        Task<Guest> insertGuest(Guest newGuest);
    }

    public class GuestService : IGuestService
    {
        private readonly WebApiDbContext _context;

        public GuestService(WebApiDbContext context) => _context = context;
        

        public async Task<Guest> insertGuest(Guest newGuest)
        {
            _context.Guests.Add(newGuest);
            await _context.SaveChangesAsync();

            return newGuest;
        }
    }
}
