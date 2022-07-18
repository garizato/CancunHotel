using CancunHotel.DataContext;
using CancunHotel.Models;
using Microsoft.Extensions.Logging;

namespace CancunHotel.Services

{
    public interface IReservationService
    {
        
        Task<String> MakeReservation(Guest guest, DateTime startDate, DateTime endDate);
    }

    public class ReservationService : IReservationService
    {
        private readonly WebApiDbContext _context;

        public ReservationService(WebApiDbContext context) => _context = context;

        public async Task<String> MakeReservation(Guest guest, DateTime startDateRequired, DateTime finalDateRequired)
        {
            var resultMessage = String.Empty;

            try
            {
                int allowedDays = 3;
                int requiredDays = 0;

                requiredDays = (int)(finalDateRequired.Date - startDateRequired.Date).TotalDays;

                if (requiredDays > allowedDays)
                {
                    resultMessage = "the duration required is greater than that available";
                }
            }
            catch (Exception)
            {
                throw; 
            }

            return resultMessage;
        }

        public bool verifyRooms(DateTime startDateRequired, DateTime finalDateRequired, out String message)
        {
            message = String.Empty;
            bool available = false;
            Room availableRoom = new Room();

            foreach (Room room in _context.Rooms)
            {
                if (room.CurrentStatus.Equals(Dictionary.RoomStatus.Available))
                {
                    availableRoom = room;
                    available = true;
                    break;
                }
            }

            if(available)
            {
                int occupation = 0;
                occupation  = _context.Reservations.Where(
                    r => r.RoomId == availableRoom.RoomId &&
                         !(startDateRequired < r.StartDate &&
                           finalDateRequired <= r.StartDate ||
                           startDateRequired >= r.FinalDate
                         )
                    ).Count();

                if(occupation > 0) { return false; }
            }

            return available;
        }
    }
}
