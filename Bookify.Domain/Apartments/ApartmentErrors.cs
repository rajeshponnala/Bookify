using Bookify.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.Apartments
{
    public static class ApartmentErrors
    {
        public static Error NotFound = new("Apartment.Found", "The booking with the specified identifier was not found.");
        
    }
}
