using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.UnitTests.Apartments
{
    internal static class ApartmentData
    {
        public static Apartment Create(Money price, Money? cleaningFee = null) => new(
              Guid.NewGuid(),
              new Name("Test apartment"),
              new Description("Test description"),
              new Address("Country", "State", "ZipCode", "City", "Street"),
              price,
              cleaningFee ?? Money.Zero(),
              null,
              amenities: new()
            );

    }
}
