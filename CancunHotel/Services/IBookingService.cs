using CancunHotel.DataContext;
using CancunHotel.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CancunHotel.Services

{
    public interface IBookingService
    {
        
        Task<String> insertBooking(Guest guest, DateTime startDate, DateTime endDate);
        Task<String> cancelBooking(int bookingId);
        Task<String> updateBooking(int bookingId, DateTime startDate, DateTime endDate);
    }

    public class BookingService : IBookingService
    {
        private readonly WebApiDbContext _context;
        private readonly IRoomService _roomService;

        public BookingService(WebApiDbContext context, IRoomService roomService) { 
            _context = context;
            _roomService = roomService;
        }

        /// <summary>
        /// Insert Booking
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="startDateRequired"></param>
        /// <param name="finalDateRequired"></param>
        /// <returns></returns>
        public async Task<String> insertBooking(Guest guest, DateTime startDateRequired, DateTime finalDateRequired)
        {
            var resultMessage = String.Empty;

            try
            {
                resultMessage = validateBookingPeriodRequested(startDateRequired, finalDateRequired);
                if(resultMessage != String.Empty)
                {
                    return resultMessage;
                }

                //get available room
                Room selectedRoom = new Room();
                if (_context.Rooms.Count() > 0)
                {
                    selectedRoom = _roomService.getRoom(startDateRequired, finalDateRequired);
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
                    //INSERT BOOKING
                    _context.Bookings.Add(newBooking);
                    await _context.SaveChangesAsync();
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
        }

        /// <summary>
        /// Validate Requested Period
        /// </summary>
        /// <param name="startDateRequired"></param>
        /// <param name="finalDateRequired"></param>
        /// <returns></returns>
        public string validateBookingPeriodRequested(DateTime startDateRequired, DateTime finalDateRequired)
        {

            string resultMessage = string.Empty;
            int requiredDays = 0;

            requiredDays = (int) (finalDateRequired.AddDays(1).Date - startDateRequired.Date).TotalDays;

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

            //validate days in advance booking
            if (startDateRequired.Date > DateTime.Now.Date.AddDays(BookingRules.daysInAdvanceReservation))
            {
                resultMessage = $"the booking start date is a maximum of {BookingRules.daysInAdvanceReservation} days";
                return resultMessage;
            }

            return resultMessage;

        }

        /// <summary>
        /// Booking Cancel
        /// </summary>
        /// <param name="bookingID"></param>
        /// <returns></returns>
        public async Task<String> cancelBooking(int bookingId)
        {
            String resultMessage = String.Empty;
            Booking bookingToCancel = _context.Bookings.Where(b => b.BookingId == bookingId).First();
            bookingToCancel.CurrentStatus = BookingStatus.cancelled;
            await _context.SaveChangesAsync();
            resultMessage = $"Your booking {bookingToCancel.BookingId} has been canceled";
            return resultMessage;
        }

        /// <summary>
        /// Booking Update
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="startDateRequired"></param>
        /// <param name="finalDateRequired"></param>
        /// <returns></returns>
        public async Task<String> updateBooking(int bookingId, DateTime startDateRequired, DateTime finalDateRequired)
        {
            String resultMessage = String.Empty;

            Booking bookingToModify = _context.Bookings.Where(b => b.BookingId == bookingId).First();

            resultMessage = validateBookingPeriodRequested(startDateRequired, finalDateRequired);

            if(resultMessage != String.Empty)
            {
                return resultMessage;
            }

            //provisionally, the status of the current reservation is set to pending so as not to affect the rescheduling
            bookingToModify.CurrentStatus = BookingStatus.pending;
            await _context.SaveChangesAsync();

            Room selectedRoom = _roomService.getRoom(startDateRequired, finalDateRequired);

            if(selectedRoom.RoomId > 0)
            {
                if(bookingToModify.StartDate == startDateRequired && bookingToModify.FinalDate == finalDateRequired) 
                {
                    bookingToModify.CurrentStatus = BookingStatus.active;
                    await _context.SaveChangesAsync();
                    resultMessage = "No changes to the booking";
                    return resultMessage;
                }

                bookingToModify.StartDate = startDateRequired;
                bookingToModify.FinalDate = finalDateRequired;
                bookingToModify.CurrentStatus = BookingStatus.active;
                await _context.SaveChangesAsync();
                resultMessage = "Booking has been Updated";
            }
            else
            {
                bookingToModify.CurrentStatus = BookingStatus.active;
                await _context.SaveChangesAsync();
                resultMessage = "No rooms available";
            }

            return resultMessage;
        }


    }
}
