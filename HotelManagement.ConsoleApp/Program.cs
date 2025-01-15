using System;
using CommandLine;
using HotelManagement.Core.Services;

namespace HotelManagement.ConsoleApp
{
    class Program
    {
        public class Options
        {
            [Option('h', "hotels", Required = true, HelpText = "Path to the hotels data file.")]
            public required string Hotels { get; set; }

            [Option('b', "bookings", Required = true, HelpText = "Path to the bookings data file.")]
            public required string Bookings { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(@"
 __        __   _                          
 \ \      / /__| | ___ ___  _ __ ___   ___ 
  \ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \
   \ V  V /  __/ | (_| (_) | | | | | |  __/
    \_/\_/ \___|_|\___\___/|_| |_| |_|\___|
                                                                            
Welcome to the Hotel Management System!

Type 'help' to see all available commands.");

            // Parse the command-line arguments
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    try
                    {
                        // Load hotel and booking data
                        var hotels = FileLoader.LoadHotels(options.Hotels);
                        var bookings = FileLoader.LoadBookings(options.Bookings);

                        var manager = new HotelManager(hotels, bookings);

                        while (true)
                        {
                            var input = Console.ReadLine();
                            
                            // Exit the program if the user types an empty line
                            if (string.IsNullOrWhiteSpace(input)) break;
                            
                            try
                            {
                                var output = UserInputHandler.HandleCommand(manager, input);
                                Console.WriteLine(output);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                })
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error: {error}");
                    }
                });
        }
    }
}
