using System;
using System.Globalization;
using System.Text;

namespace HotelBookingSorter
{
    public static class Program
    {
        private static readonly Sorter _sorter = new Sorter();

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            PrintWelcome();

            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = Console.ReadLine()?.Trim() ?? string.Empty;
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        HandleAddRecord();
                        break;
                    case "2":
                        HandleRemoveRecord();
                        break;
                    case "3":
                        _sorter.PrintCollection();
                        break;
                    case "4":
                        _sorter.GenerateControlData();
                        break;
                    case "5":
                        _sorter.SortCollection();
                        break;
                    case "6":
                        _sorter.PrintIntermediateSteps();
                        break;
                    case "7":
                        _sorter.PrintStatistics();
                        break;
                    case "8":
                        _sorter.PrintTopExpensive(5);
                        break;
                    case "9":
                        HandlePriceRange();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Unknown option. Please enter a number from the menu.");
                        break;
                }
            }
        }

        private static void PrintWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("Hotel Booking Sorter");
            Console.WriteLine("Algorithm: Quick Sort");
            Console.WriteLine();
        }

        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("MENU");
            Console.WriteLine("1. Add booking record");
            Console.WriteLine("2. Remove booking record");
            Console.WriteLine("3. Print collection");
            Console.WriteLine("4. Generate control data (12 records)");
            Console.WriteLine("5. Sort collection (Quick Sort)");
            Console.WriteLine("6. Print intermediate sort steps");
            Console.WriteLine("7. Print sort statistics");
            Console.WriteLine("8. Top 5 most expensive bookings");
            Console.WriteLine("9. Bookings within a price range");
            Console.WriteLine("0. Exit");
            Console.Write("Your choice: ");
        }

        private static void HandleAddRecord()
        {
            Console.WriteLine("Add New Booking Record");

            int? id = ReadInt("Booking ID (positive integer): ", 1, int.MaxValue);
            if (id is null)
            {
                return;
            }

            Console.Write("Guest surname (non-empty): ");
            string surname = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(surname))
            {
                Console.WriteLine("Error: surname cannot be empty.");
                return;
            }

            int? nights = ReadInt("Number of nights (1-365): ", 1, 365);
            if (nights is null)
            {
                return;
            }

            decimal? cost = ReadDecimal("Total cost in UAH (> 0): ", 0.01m);
            if (cost is null)
            {
                return;
            }

            _sorter.AddRecord(new BookingRecord(id.Value, surname, nights.Value, cost.Value));
            Console.WriteLine("Record added successfully.");
        }

        private static void HandleRemoveRecord()
        {
            Console.Write("Enter Booking ID to remove: ");
            string raw = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(raw, out int id))
            {
                Console.WriteLine("Error: invalid ID - must be an integer.");
                return;
            }

            _sorter.RemoveRecord(id);
        }

        private static void HandlePriceRange()
        {
            Console.WriteLine("Filter Bookings by Price Range");

            decimal? min = ReadDecimal("Minimum cost (>= 0): ", 0m);
            if (min is null)
            {
                return;
            }

            decimal? max = ReadDecimal($"Maximum cost (>= {min.Value:F2}): ", min.Value);
            if (max is null)
            {
                return;
            }

            _sorter.PrintInPriceRange(min.Value, max.Value);
        }

        private static int? ReadInt(string prompt, int min, int max)
        {
            for (int attempt = 0; attempt < 3; attempt++)
            {
                Console.Write(prompt);
                string raw = Console.ReadLine()?.Trim() ?? string.Empty;

                if (int.TryParse(raw, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Error: please enter an integer between {min} and {max}.");
            }

            Console.WriteLine("Operation cancelled after too many invalid inputs.");
            return null;
        }

        private static decimal? ReadDecimal(string prompt, decimal min)
        {
            for (int attempt = 0; attempt < 3; attempt++)
            {
                Console.Write(prompt);
                string raw = (Console.ReadLine()?.Trim() ?? string.Empty).Replace(',', '.');

                if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value) &&
                    value >= min)
                {
                    return value;
                }

                Console.WriteLine($"Error: please enter a number >= {min:F2}.");
            }

            Console.WriteLine("Operation cancelled after too many invalid inputs.");
            return null;
        }
    }
}