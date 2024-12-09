using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.UnitTests.Apartments
{
    internal class ApartmentData
    {
        public static Apartment Create() => new(
             Guid.NewGuid(),
             new Name("Test apartment"),
             new Description("Test description"),
             new Address("Country", "State", "ZipCode", "City", "Street"),
             new Money(100.0m, Currency.Usd),
             Money.Zero(),
             null,
             amenities: new()
           );
    }
}
