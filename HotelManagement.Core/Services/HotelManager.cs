using HotelManagement.Core.Models;

namespace HotelManagement.Core.Services
{
    public class HotelManager(List<Hotel> hotels, List<Booking> bookings)
    {
        public string GetRoomAvailability(string hotelId, DateTime date, string roomType)
        {
            var hotel = hotels.FirstOrDefault(h => h.Id == hotelId);
            if (hotel == null) throw new Exception("Hotel not found.");

            var totalRooms = hotel.Rooms.Count(r => r.RoomType == roomType);
            var bookedRooms = bookings.Count(b =>
                b.HotelId == hotelId &&
                b.RoomType == roomType &&
                b.Arrival <= date &&
                b.Departure > date);

            return $"Available rooms: {totalRooms - bookedRooms}";
        }

        public List<string> AllocateRooms(string hotelId, DateTime date, int peopleCount)
        {
            var hotel = hotels.FirstOrDefault(h => h.Id == hotelId);
            if (hotel == null) throw new Exception("Hotel not found.");

            var availableRooms = hotel.Rooms
                .Where(r => !IsRoomBooked(hotelId, r.RoomId, date, date))
                .Select(r => r.RoomType)
                .ToList();

            return AllocateRooms(availableRooms, peopleCount);
        }

        private bool IsRoomBooked(string hotelId, string roomId, DateTime startDate, DateTime endDate)
        {
            return bookings.Any(b =>
                b.HotelId == hotelId &&
                b.Arrival < endDate &&
                b.Departure > startDate &&
                b.RoomType == roomId);
        }

        private List<string> AllocateRooms(List<string> availableRooms, int peopleCount)
        {
            // Prioritize DBL (double) rooms over SGL (single) rooms
            var dblRooms = availableRooms.Where(r => r == "DBL").ToList();
            var sglRooms = availableRooms.Where(r => r == "SGL").ToList();

            const int dblCapacity = 2;
            const int sglCapacity = 1;
            var result = new List<string>();
            int remaining = peopleCount;

            foreach (var roomType in dblRooms)
            {
                if (remaining <= 0) break;
                if (remaining >= dblCapacity)
                {
                    result.Add(roomType);
                    remaining -= dblCapacity;
                }
                else
                {
                    result.Add(roomType + "!");
                    remaining = 0;
                }
            }

            foreach (var roomType in sglRooms)
            {
                if (remaining <= 0) break;
                result.Add(roomType);
                remaining -= sglCapacity;
            }

            if (remaining > 0) throw new Exception("Not enough rooms to accommodate the request.");
            return result;
        }
    }
}