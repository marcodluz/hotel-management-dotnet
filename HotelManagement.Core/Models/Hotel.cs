namespace HotelManagement.Core.Models
{
    public class Hotel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required List<RoomType> RoomTypes { get; set; }
        public required List<Room> Rooms { get; set; }
    }
}