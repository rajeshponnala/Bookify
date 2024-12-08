using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookify.Domain.UnitTests.Bookings
{
    public class PricingServiceTest
    {
        private static readonly PricingService pricingService = new();
        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPrice()
        {
            var price = new Money(10.0m, Currency.Usd);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
            var apartment = ApartmentData.Create(price);
            var pricingDetails = pricingService.CalculatePrice(apartment, period);
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }
        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
        {
            var price = new Money(10.0m, Currency.Usd);
            var cleaningFee = new Money(99.99m, Currency.Usd);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money(price.Amount * period.LengthInDays + cleaningFee.Amount, price.Currency);
            var apartment = ApartmentData.Create(price, cleaningFee);
            var pricingDetails = pricingService.CalculatePrice(apartment, period);
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }
    }
}
