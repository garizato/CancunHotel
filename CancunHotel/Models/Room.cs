namespace CancunHotel.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int MaxDaysInAdvance { get; set; }
        public Dictionary.RoomStatus CurrentStatus { get; set; }

    }
}

