namespace HotelManagement.Core.Models
{
    public class Booking
    {
        public required string HotelId { get; set; }
        public required DateTime Arrival { get; set; }
        public required DateTime Departure { get; set; }
        public required string RoomType { get; set; }
    }
}