using CancunHotel.DataContext;
using CancunHotel.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CancunHotel.Services

{
    public interface IBookingService
    {
        
        Task<String> insertBooking(Guest guest, DateTime startDate, DateTime endDate);
    }

    public class BookingService : IBookingService
    {
        private readonly WebApiDbContext _context;

        public BookingService(WebApiDbContext context) => _context = context;

        public async Task<String> insertBooking(Guest guest, DateTime startDateRequired, DateTime finalDateRequired)
        {
            var resultMessage = String.Empty;

            try
            {
                //DateTime startDateRequired;
                //if(DateTime.TryParseExact(strStartDateRequired, "dd/MM/AAAA", new CultureInfo("en-US"), DateTimeStyles.None, out startDateRequired)) 
                //{ 
                //    resultMessage = "invalid format start date";
                //    return resultMessage;
                //}
                //DateTime finalDateRequired;
                //if (DateTime.TryParseExact(strFinalDateRequired, "dd/MM/AAAA", new CultureInfo("en-US"), DateTimeStyles.None, out finalDateRequired))
                //{
                //    resultMessage = "invalid format final date";
                //    return resultMessage;
                //}

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
                    resultMessage = $"the requested booking date is not valid";
                    return resultMessage;
                } 

                if(startDateRequired.Date > DateTime.Now.Date.AddDays(BookingRules.daysInAdvanceReservation))
                {
                    resultMessage = $"the booking start date is a maximum of {BookingRules.daysInAdvanceReservation} days";
                    return resultMessage;
                }

                Room selectedRoom = new Room();
                if (_context.Rooms.Count() > 0)
                {
                    selectedRoom = getRoom(startDateRequired, finalDateRequired);
                } else
                {
                    resultMessage = "there are no rooms loaded in the system";
                    return resultMessage;
                }

                if(selectedRoom.RoomId > 0)
                {

                    Booking newBooking = new Booking()
                    {
                        RoomId = selectedRoom.RoomId,
                        GuestId = guest.GuestId,
                        StartDate = startDateRequired,
                        FinalDate = finalDateRequired,
                        CurrentStatus = BookingStatus.active
                    };
                    //TODO: INSERT BOOKING
                    _context.Bookings.Add(newBooking);
                    _context.SaveChanges();
                    resultMessage = $"Congratulations your booking #{newBooking.BookingId} has been successful. Assigned room {selectedRoom.RoomNumber}";
                    return resultMessage;
                } else
                {
                    resultMessage = "No rooms available";
                    return resultMessage;
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
