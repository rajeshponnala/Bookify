using Bookify.Application.Apartments.SearchApartments;
using Bookify.Application.IntegrationTests.Infrastructure;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookify.Application.IntegrationTests.Apartments
{
    public class SearchApartmentsTests : BaseIntegrationTest
    {
        public SearchApartmentsTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task SearchApartments_ShouldReturnEmptyList_WhenDateRangeIsInvalid()
        {
            var query = new SearchApartmentsQuery(new DateOnly(2024,1,10), new DateOnly(2024,1,1));

            var result = await Sender.Send(query, default);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchApartments_ShouldReturnApartments_WhenDateRangeIsValid()
        {
            var query = new SearchApartmentsQuery(new DateOnly(2024, 1, 10), new DateOnly(2024, 2, 10));

            var result = await Sender.Send(query, default);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
        }
    }
}
