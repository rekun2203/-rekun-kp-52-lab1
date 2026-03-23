using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSorter
{
    public class Sorter
    {
        private List<BookingRecord> _collection;
        private readonly SortStatistics _statistics;
        private List<string> _intermediateSteps;
        private bool _isSorted;
        private bool _sortExecuted;

        public Sorter()
        {
            _statistics = new SortStatistics();
            _intermediateSteps = new List<string>();
            _isSorted = false;
            _sortExecuted = false;
            InitCollection();
        }

        public void InitCollection()
        {
            _collection = new List<BookingRecord>();
            _intermediateSteps = new List<string>();
            _statistics.Reset();
            _isSorted = false;
            _sortExecuted = false;
        }

        public void AddRecord(BookingRecord record)
        {
            _collection.Add(record);
            _isSorted = false;
            _sortExecuted = false;
            _intermediateSteps.Clear();
        }

        public void RemoveRecord(int bookingId)
        {
            for (int i = 0; i < _collection.Count; i++)
            {
                if (_collection[i].BookingId == bookingId)
                {
                    _collection.RemoveAt(i);
                    Console.WriteLine($"Record with ID {bookingId} removed successfully.");
                    _isSorted = false;
                    _sortExecuted = false;
                    _intermediateSteps.Clear();
                    return;
                }
            }

            Console.WriteLine($"Record with ID {bookingId} was not found.");
        }

        public void PrintCollection()
        {
            if (_collection.Count == 0)
            {
                Console.WriteLine("Collection is empty.");
                return;
            }

            string sortedLabel = _isSorted ? " [sorted]" : " [unsorted]";
            Console.WriteLine($"Collection ({_collection.Count} records){sortedLabel}");

            for (int i = 0; i < _collection.Count; i++)
            {
                Console.WriteLine($"{i + 1,3}. {_collection[i]}");
            }
        }

        public void GenerateControlData()
        {
            InitCollection();

            BookingRecord[] controlData =
            {
                new BookingRecord(1001, "Anderson", 3, 4500.00m),
                new BookingRecord(1002, "Brown", 7, 12600.00m),
                new BookingRecord(1003, "Clark", 2, 1800.00m),
                new BookingRecord(1004, "Davis", 5, 9750.00m),
                new BookingRecord(1005, "Evans", 1, 950.00m),
                new BookingRecord(1006, "Foster", 10, 18000.00m),
                new BookingRecord(1007, "Garcia", 4, 7200.00m),
                new BookingRecord(1008, "Harris", 3, 4500.00m),
                new BookingRecord(1009, "Ingram", 6, 10800.00m),
                new BookingRecord(1010, "Johnson", 2, 2400.00m),
                new BookingRecord(1011, "King", 8, 14400.00m),
                new BookingRecord(1012, "Lewis", 3, 4500.00m),
            };

            for (int i = 0; i < controlData.Length; i++)
            {
                _collection.Add(controlData[i]);
            }

            Console.WriteLine("Control data generated: 12 records loaded.");
        }

        public void SortCollection()
        {
            if (_collection.Count == 0)
            {
                Console.WriteLine("Collection is empty. Nothing to sort.");
                return;
            }

            _statistics.Reset();
            _intermediateSteps = new List<string>();

            BookingRecord[] array = new BookingRecord[_collection.Count];
            for (int i = 0; i < _collection.Count; i++)
            {
                array[i] = _collection[i];
            }

            _statistics.StartTimer();
            QuickSort(array, 0, array.Length - 1);
            _statistics.StopTimer();

            _collection = new List<BookingRecord>();
            for (int i = 0; i < array.Length; i++)
            {
                _collection.Add(array[i]);
            }

            _isSorted = true;
            _sortExecuted = true;

            Console.WriteLine("Collection sorted successfully by Total Cost (desc), then Surname (asc).");
        }

        public void PrintIntermediateSteps()
        {
            if (!_sortExecuted)
            {
                Console.WriteLine("No steps recorded. Run the sort first (option 5).");
                return;
            }

            if (_intermediateSteps.Count == 0)
            {
                Console.WriteLine("No partitions performed (collection had 0 or 1 element).");
                return;
            }

            Console.WriteLine($"Intermediate Quick Sort Steps ({_intermediateSteps.Count} partitions)");
            Console.WriteLine("Format: Step | Range [L..R] | Pivot (Surname, Cost) | Costs in subarray");

            for (int i = 0; i < _intermediateSteps.Count; i++)
            {
                Console.WriteLine($"Step {i + 1,3}: {_intermediateSteps[i]}");
            }
        }

        public void PrintStatistics()
        {
            if (!_sortExecuted)
            {
                Console.WriteLine("No statistics available. Run the sort first (option 5).");
                return;
            }

            _statistics.Print();
        }

        public void PrintTopExpensive(int count)
        {
            if (!RequireSorted())
            {
                return;
            }

            int limit = count < _collection.Count ? count : _collection.Count;
            Console.WriteLine($"Top {limit} Most Expensive Bookings");

            for (int i = 0; i < limit; i++)
            {
                int index = _collection.Count - 1 - i;
                Console.WriteLine($"{i + 1,3}. {_collection[index]}");
            }
        }

        public void PrintInPriceRange(decimal minCost, decimal maxCost)
        {
            if (!RequireSorted())
            {
                return;
            }

            Console.WriteLine($"Bookings with cost in [{minCost:F2} - {maxCost:F2}] UAH");
            int found = 0;

            for (int i = 0; i < _collection.Count; i++)
            {
                decimal cost = _collection[i].TotalCost;
                if (cost >= minCost && cost <= maxCost)
                {
                    found++;
                    Console.WriteLine($"{found,3}. {_collection[i]}");
                }
            }

            if (found == 0)
            {
                Console.WriteLine("No bookings found in this price range.");
            }
            else
            {
                Console.WriteLine($"Total bookings in range: {found}");
            }
        }

        private void QuickSort(BookingRecord[] array, int left, int right)
        {
            _statistics.IncrementRecursiveCalls();

            if (left >= right)
            {
                return;
            }

            int pivotIndex = Partition(array, left, right);
            QuickSort(array, left, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, right);
        }

        private int Partition(BookingRecord[] array, int left, int right)
        {
            BookingRecord pivot = array[right];

            _intermediateSteps.Add(
                $"Range [{left,2}..{right,2}] | Pivot: ({pivot.GuestSurname}, {pivot.TotalCost:F2}) | Costs: {BuildCostSnapshot(array, left, right)}");

            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                _statistics.IncrementComparisons();

                if (Compare(array[j], pivot) >= 0)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, right);
            return i + 1;
        }

        private int Compare(BookingRecord a, BookingRecord b)
        {
            if (a.TotalCost != b.TotalCost)
            {
                return b.TotalCost.CompareTo(a.TotalCost);
            }

            return string.Compare(a.GuestSurname, b.GuestSurname, StringComparison.Ordinal);
        }

        private void Swap(BookingRecord[] array, int i, int j)
        {
            if (i == j)
            {
                return;
            }

            BookingRecord temp = array[i];
            array[i] = array[j];
            array[j] = temp;
            _statistics.IncrementSwaps();
        }

        private string BuildCostSnapshot(BookingRecord[] array, int left, int right)
        {
            StringBuilder sb = new StringBuilder("[");

            for (int i = left; i <= right; i++)
            {
                if (i > left)
                {
                    sb.Append(", ");
                }

                sb.Append($"{array[i].TotalCost:F0}");
            }

            sb.Append("]");
            return sb.ToString();
        }

        private bool RequireSorted()
        {
            if (!_isSorted)
            {
                Console.WriteLine("Collection is not yet sorted. Please run Sort (option 5) first.");
                return false;
            }

            return true;
        }
    }
}