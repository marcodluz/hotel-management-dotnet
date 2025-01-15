namespace HotelManagement.Core.Models
{
    public class RoomType
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public required List<string> Amenities { get; set; }
        public required List<string> Features { get; set; }
    }
}