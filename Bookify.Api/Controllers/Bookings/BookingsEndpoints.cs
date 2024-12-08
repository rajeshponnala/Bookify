using Asp.Versioning;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bookify.Api.Controllers.Bookings
{
    public static class BookingsEndpoints
    {
        public static IEndpointRouteBuilder MapBookingEndPoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("bookings/{id}", GetBooking)
                .RequireAuthorization()
                .WithName(nameof(GetBooking));

            builder.MapPost("bookings",ReserveBooking)
                .RequireAuthorization();
            return builder;
        }
        public static async Task<IResult> GetBooking(Guid id,ISender sender,CancellationToken cancellationToken)
        {
            var query = new GetBookingQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }
        public static async Task<IResult> ReserveBooking(
            ReserveBookingRequest request,
            ISender sender,
            HttpContext context,
            CancellationToken cancellationToken)
        {
            Guid.TryParse(
                context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value.ToString(),
                out Guid loggedInUserId);

            var command = new ReserveBookingCommand(
                  request.ApartmentId,
                  loggedInUserId,
                  request.StartDate,
                  request.EndDate
                );
            var result = await sender.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.CreatedAtRoute(nameof(GetBooking), new { id = result.Value }, result.Value);
        }
    }
}
