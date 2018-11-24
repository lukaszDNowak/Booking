using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingApp
{
   public class BookingHelper
    {
        public static string OverlappingBookingsExist(Booking booking, IBookingRepository _repository)
        {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var bookings = _repository.GetActiveBookings(1);

            bookings.Where(b => b.Id != booking.Id && b.Status != "Cancelled");
            
            var overlappingBooking =
            bookings.FirstOrDefault(
            b =>
                booking.ArrivalDate >= b.ArrivalDate
                && booking.ArrivalDate < b.DepartureDate
                || booking.DepartureDate > b.ArrivalDate
                && booking.DepartureDate <= b.DepartureDate);
            
            return overlappingBooking == null ? string.Empty
            : overlappingBooking.Reference;
        }
    }
}

