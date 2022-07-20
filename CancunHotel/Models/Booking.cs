namespace CancunHotel.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public BookingStatus CurrentStatus { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum BookingStatus
    {
        cancelled,
        active,
        finished,
        pending
    }
}
