using System;

namespace HotelBookingSorter
{
    public class BookingRecord
    {
        public int BookingId { get; set; }
        public string GuestSurname { get; set; }
        public int Nights { get; set; }
        public decimal TotalCost { get; set; }

        public BookingRecord(int bookingId, string guestSurname, int nights, decimal totalCost)
        {
            BookingId = bookingId;
            GuestSurname = guestSurname;
            Nights = nights;
            TotalCost = totalCost;
        }

        public override string ToString()
        {
            return $"ID: {BookingId,-6} | Guest: {GuestSurname,-15} | Nights: {Nights,-3} | Cost: {TotalCost,10:F2} UAH";
        }
    }
}