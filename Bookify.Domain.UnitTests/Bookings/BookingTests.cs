using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookify.Domain.UnitTests.Bookings
{
    public class BookingTests: BaseTest
    {
        private static readonly PricingService pricingService = new();
        [Fact]
        public void Reserve_Should_RaiseBookingReservedDomainEvent()
        {
            var price = new Money(10.0m, Currency.Usd);
            var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var apartment = ApartmentData.Create(price);
            var pricingDetails = pricingService.CalculatePrice(apartment, period);
            var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);

            var domainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);
            domainEvent.BookingId.Should().Be(booking.Id);
        }

    }
}
