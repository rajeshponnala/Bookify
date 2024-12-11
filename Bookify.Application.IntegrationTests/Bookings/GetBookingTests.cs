using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Bookings;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookify.Application.IntegrationTests.Bookings
{
    public class GetBookingTests : BaseIntegrationTest
    {
        private static readonly Guid BookingId = Guid.NewGuid();
        public GetBookingTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetBooking_ShouldReturnFailure_WhenBookingIsNotFound()
        {
            var command = new GetBookingQuery(BookingId);

            var result = await Sender.Send(command);

            result.Error.Should().Be(BookingErrors.NotFound);
        }
    }
}
