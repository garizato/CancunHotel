﻿namespace CancunHotel.Models
{
    public class Booking
    {
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public BookingStatus CurrentStatus { get; set; }
    }

    public enum BookingStatus
    {
        cancelled,
        active
    }
}