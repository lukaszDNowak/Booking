using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingApp
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
    }
}
