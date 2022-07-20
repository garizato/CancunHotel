using CancunHotel.DataContext;
using CancunHotel.Models;

namespace CancunHotel.Services
{
    public interface IRoomService
    {
        Room getRoom(DateTime startDateRequired, DateTime finalDateRequired);
        Task<String> checkAvailableRoom(int roomNumber);
    }

    public class RoomService : IRoomService
    {
        private readonly WebApiDbContext _context;

        public RoomService(WebApiDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Available Room
        /// </summary>
        /// <param name="startDateRequired"></param>
        /// <param name="finalDateRequired"></param>
        /// <returns></returns>
        public Room getRoom(DateTime startDateRequired, DateTime finalDateRequired)
        {
            Room availableRoom = new Room();
            int occupation = 0;

            foreach (Room room in _context.Rooms)
            {
                occupation = _context.Bookings.Where(
                    r => r.RoomId == room.RoomId &&
                         !(startDateRequired < r.StartDate &&
                           finalDateRequired <= r.StartDate ||
                           startDateRequired >= r.FinalDate.AddHours(23)
                         ) && r.CurrentStatus == BookingStatus.active
                    ).Count();

                if (occupation == 0)
                {
                    availableRoom = room;
                    break;
                }
            }

            return availableRoom;
        }

        public async Task<String> checkAvailableRoom(int roomNumber)
        {
            List<Booking> bookings = new List<Booking>();
            string resultMessage = string.Empty;

            Room checkingRoom = _context.Rooms.Where(r => r.RoomNumber == roomNumber).FirstOrDefault();

            bookings = _context.Bookings.Where(
                    r => r.RoomId == checkingRoom.RoomId && r.CurrentStatus == BookingStatus.active
                    ).OrderByDescending(r => r.StartDate).ToList();

            if(bookings.Count > 0)
            {
                resultMessage = "the room is occupied the following dates:\n";
                foreach (Booking booking in bookings)
                {
                    resultMessage += $"From: {booking.StartDate.Date} To: {booking.FinalDate.Date}\n";
                }
            }

            return resultMessage;
        }
    }
}
