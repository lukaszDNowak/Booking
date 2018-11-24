using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingApp
{
    public class UnitOfWork
    {
        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }
    }
}
