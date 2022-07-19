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
                //validate days of stay requested
                
                int requiredDays = 0;

                requiredDays = (int)(finalDateRequired.Date - startDateRequired.Date).TotalDays;

                if (requiredDays > BookingRules.maximumDaysOfStay)
                {
                    resultMessage = "the duration required is greater than that available";
                    return resultMessage;
                }

                //validate start day required

                if(startDateRequired.Date <= DateTime.Now.Date)
                {
                    resultMessage = "the requested reservation date is not valid";
                    return resultMessage;
                } 

                if(startDateRequired.Date > DateTime.Now.Date.AddDays(BookingRules.daysInAdvanceReservation))
                {
                    resultMessage = $"the reservation start date is a maximum of {BookingRules.daysInAdvanceReservation} days";
                    return resultMessage;
                }

                Room selectedRoom = new Room();

                selectedRoom = getRoom(startDateRequired, finalDateRequired);

                if(selectedRoom.RoomId > 0)
                {
                    //TODO: INSERT BOOKING
                    resultMessage = $"Congratulations your reservation has been successful. Assigned room {selectedRoom.RoomNumber}";
                }

            }
            catch (Exception)
            {
                throw; 
            }

            return resultMessage;
        }

        public Room getRoom(DateTime startDateRequired, DateTime finalDateRequired)
        {
            Room availableRoom = new Room();
            int occupation = 0;

            //The requirement is for one room only but logic develops if more are added.
            foreach (Room room in _context.Rooms)
            {
                //validate availability
                occupation = _context.Bookings.Where(
                    r => r.RoomId == room.RoomId &&
                         !(startDateRequired < r.StartDate &&
                           finalDateRequired <= r.StartDate ||
                           startDateRequired >= r.FinalDate
                         )
                    ).Count();

                if (occupation == 0) 
                {
                    availableRoom = room;
                    break;
                }
            }

            return availableRoom;
        }
    }
}
