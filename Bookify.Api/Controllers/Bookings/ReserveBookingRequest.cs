namespace Bookify.Api.Controllers.Bookings
{
    public record ReserveBookingRequest(Guid ApartmentId,
        DateOnly StartDate,
        DateOnly EndDate);
}
