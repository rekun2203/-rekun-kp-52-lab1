using System;
using System.Diagnostics;

namespace HotelBookingSorter
{
    public class SortStatistics
    {
        private int _comparisons;
        private int _swaps;
        private int _recursiveCalls;
        private readonly Stopwatch _stopwatch;

        public int Comparisons => _comparisons;
        public int Swaps => _swaps;
        public int RecursiveCalls => _recursiveCalls;
        public TimeSpan ElapsedTime => _stopwatch.Elapsed;

        public SortStatistics()
        {
            _stopwatch = new Stopwatch();
        }

        public void Reset()
        {
            _comparisons = 0;
            _swaps = 0;
            _recursiveCalls = 0;
            _stopwatch.Reset();
        }

        public void IncrementComparisons()
        {
            _comparisons++;
        }

        public void IncrementSwaps()
        {
            _swaps++;
        }

        public void IncrementRecursiveCalls()
        {
            _recursiveCalls++;
        }

        public void StartTimer()
        {
            _stopwatch.Start();
        }

        public void StopTimer()
        {
            _stopwatch.Stop();
        }

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine("Algorithm Statistics");
            Console.WriteLine($"Comparisons:     {_comparisons}");
            Console.WriteLine($"Swaps:           {_swaps}");
            Console.WriteLine($"Recursive calls: {_recursiveCalls}");
            Console.WriteLine($"Execution time:   {_stopwatch.Elapsed.TotalMilliseconds:F4} ms");
            Console.WriteLine();
        }
    }
}