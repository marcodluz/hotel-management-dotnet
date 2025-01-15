using Newtonsoft.Json;
using HotelManagement.Core.Models;

public static class FileLoader
{
    public static List<Hotel> LoadHotels(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<Hotel>>(json) ?? [];
    }

    public static List<Booking> LoadBookings(string filePath)
    {
        var json = File.ReadAllText(filePath);

        var settings = new JsonSerializerSettings
        {
            DateFormatString = "yyyyMMdd"
        };

        return JsonConvert.DeserializeObject<List<Booking>>(json, settings) ?? [];
    }
}