using System;
using System.Collections.Generic;
using System.Linq;
using HotelManagement.Core.Models;
using HotelManagement.Core.Services;

public static class UserInputHandler
{
    public static string HandleCommand(HotelManager manager, string command)
    {
        if (command.StartsWith("Availability", StringComparison.OrdinalIgnoreCase))
        {
            return HandleAvailability(manager, command);
        }
        else if (command.StartsWith("RoomTypes", StringComparison.OrdinalIgnoreCase))
        {
            return HandleRoomTypes(manager, command);
        }
        else if (command.StartsWith("Help", StringComparison.OrdinalIgnoreCase))
        {
            return HandleHelp();
        }
        else
        {
            return "Invalid command. Supported commands: Availability, RoomTypes, Help";
        }
    }

    private static string HandleAvailability(HotelManager manager, string command)
    {
        try
        {
            var args = ParseCommandArguments(command, "Availability");
            var hotelId = args[0];
            var dateRange = args[1];
            var roomType = args[2];

            // Handle single date or date range
            var dates = ParseDateRange(dateRange);
            var results = new List<string>();

            foreach (var date in dates)
            {
                var availability = manager.GetRoomAvailability(hotelId, date, roomType);
                results.Add(availability);
            }

            // Return only one output
            return results.FirstOrDefault() ?? "No availability information.";
        }
        catch (Exception ex)
        {
            return $"Error processing Availability command: {ex.Message}";
        }
    }

    private static string HandleRoomTypes(HotelManager manager, string command)
    {
        try
        {
            // Parse command input
            var args = ParseCommandArguments(command, "RoomTypes");
            var hotelId = args[0];
            var dateRange = args[1];
            var peopleCount = int.Parse(args[2]);

            // Handle single date or date range
            var dates = ParseDateRange(dateRange);
            var results = new List<string>();

            foreach (var date in dates)
            {
                var roomAllocation = manager.AllocateRooms(hotelId, date, peopleCount);

                if (roomAllocation == null)
                {
                    results.Add("Error: Allocation is not possible.");
                }
                else
                {
                    results.Add(string.Join(", ", roomAllocation));
                }
            }

            // Return only one output
            return results.FirstOrDefault() ?? "No allocation information.";
        }
        catch (Exception ex)
        {
            return $"Error processing RoomTypes command: {ex.Message}";
        }
    }

    private static string HandleHelp()
    {
        var helpText = "Available Commands:\n\n";

        var commands = new List<Command>
        {
            new Command
            {
                Name = "Availability",
                Args = new List<string> { "hotelId", "dateRange", "roomType" },
                Description = "Check room availability.",
                Examples = new List<string>
                {
                    "Availability(H1, 20240901, SGL)",
                    "Availability(H1, 20240901-20240903, DBL)"
                }
            },
            new Command
            {
                Name = "RoomTypes",
                Args = new List<string> { "hotelId", "dateRange", "numPeople" },
                Description = "List available room types.",
                Examples = new List<string>
                {
                    "RoomTypes(H1, 20240904, 3)",
                    "RoomTypes(H1, 20240905-20240907, 5)"
                }
            }
        };

        foreach (var command in commands)
        {
            helpText += $"  • {command.Name}({string.Join(", ", command.Args.Select(arg => $"[{arg}]"))})\n";
            helpText += $"    {command.Description}\n";
            helpText += "    Examples:\n";
            foreach (var example in command.Examples)
            {
                helpText += $"      - {example}\n";
            }
            helpText += "\n";
        }

        return helpText;
    }

    private static List<string> ParseCommandArguments(string command, string commandName)
    {
        var argsStart = command.IndexOf('(') + 1;
        var argsEnd = command.IndexOf(')');
        if (argsStart < 0 || argsEnd < 0 || argsEnd <= argsStart)
        {
            throw new ArgumentException($"Invalid {commandName} command format.");
        }

        var args = command.Substring(argsStart, argsEnd - argsStart).Split(',');
        return args.Select(arg => arg.Trim()).ToList();
    }

    private static List<DateTime> ParseDateRange(string dateRange)
    {
        if (dateRange.Contains('-'))
        {
            var dates = dateRange.Split('-');
            var startDate = DateTime.ParseExact(dates[0], "yyyyMMdd", null);
            var endDate = DateTime.ParseExact(dates[1], "yyyyMMdd", null);

            return Enumerable.Range(0, (endDate - startDate).Days + 1)
                             .Select(offset => startDate.AddDays(offset))
                             .ToList();
        }
        else
        {
            return new List<DateTime> { DateTime.ParseExact(dateRange, "yyyyMMdd", null) };
        }
    }
}
