using System.Diagnostics.CodeAnalysis;

namespace HotelManagement.Core.Models
{
    public class Command
    {
        public required string Name { get; set; }
        public required List<string> Args { get; set; }
        public required string Description { get; set; }
        public required List<string> Examples { get; set; }

    }
}