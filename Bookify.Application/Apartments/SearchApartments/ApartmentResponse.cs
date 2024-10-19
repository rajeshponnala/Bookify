using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Apartments.SearchApartments
{
    public sealed class ApartmentResponse
    {
        public Guid Id { get;init; }
        public required string Name { get;init; }

        public required string Description { get;init; }

        public decimal Price { get;init; }


        public required string Currency { get;init; }

        public AddressResponse Address { get; set; }

    }
}
