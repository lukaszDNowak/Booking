using Moq;
using NUnit.Framework;
using BookingApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingTest   
{  
    [TestFixture] 
    public class BookingAppTest
    {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _existingBooking = new Booking
            {
                Id = 1,
                ArrivalDate = ArivalOn(2018, 11, 24),
                DepartureDate = DepartOn(2018, 11, 30),
                Reference = "ref1",
                Status = "status"
            };

            _repository = new Mock<IBookingRepository>();
            _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking>
            {
              _existingBooking
            }.AsQueryable());
        }

        [Test]
        public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 2,
                ArrivalDate = Before(_existingBooking.ArrivalDate, days: 2),
                DepartureDate = Before(_existingBooking.ArrivalDate)
            },
            _repository.Object);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsBeforeExistingBookingAndEndInBookingTime_ReturnRefString()
        {  
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 2,
                ArrivalDate = Before(_existingBooking.ArrivalDate, days: 2),
                DepartureDate = Before(_existingBooking.ArrivalDate,days:-1)
            },
            _repository.Object);
            Assert.AreEqual("ref1", result);
        }

        [Test]
        public void BookingStartsInExistingBookingAndEndInBookingTime_ReturnRefString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 2,
                ArrivalDate = _existingBooking.ArrivalDate,
                DepartureDate = _existingBooking.DepartureDate
            },
            _repository.Object);
            Assert.AreEqual("ref1", result);
        }

        [Test]
        public void BookingStartsInExistingBookingAndEndAfterBookingTime_ReturnRefString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 2,
                ArrivalDate = _existingBooking.ArrivalDate,
                DepartureDate = After(_existingBooking.DepartureDate, 2)
            },
            _repository.Object);
            Assert.AreEqual("ref1", result);
        }

        [Test]
        public void BookingStartsAndFinishesAfterAnExistingBooking_ReturnEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 2,
                ArrivalDate = After(_existingBooking.ArrivalDate, 10),
                DepartureDate = After(_existingBooking.DepartureDate, 10)
            },
            _repository.Object);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsBeforeExistingBookingAndFinishesAfterExistingBooking_ReturnRefString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 2,
                ArrivalDate = Before(_existingBooking.ArrivalDate, 10),
                DepartureDate = After(_existingBooking.DepartureDate, 10)
            },
            _repository.Object);
            Assert.AreEqual("ref1", result);
        }

        private DateTime ArivalOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime Before(DateTime arrivalDate, int days=1)
        {
            return arrivalDate.AddDays(-days);
        }

        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }

        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}
