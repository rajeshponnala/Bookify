using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Application.Exceptions;
using Bookify.Application.UnitTests.Apartments;
using Bookify.Application.UnitTests.Users;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Bookify.Application.UnitTests.Bookings
{
    public class ReserveBookingTests
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;
        private static readonly ReserveBookingCommand Command = new(
             Guid.NewGuid(),
             Guid.NewGuid(),
             new DateOnly(2024,1,1),
             new DateOnly(2024,1,10)
             );
        private readonly ReserveBookingCommandHandler _handler;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IApartmentRepository _apartmentRepositoryMock;
        private readonly IBookingRepository _bookingRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly PricingService _pricingService;
        private readonly IDateTimeProvider _datetimeProviderMock;

        public ReserveBookingTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _apartmentRepositoryMock = Substitute.For<IApartmentRepository>();
            _bookingRepositoryMock = Substitute.For<IBookingRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _datetimeProviderMock = Substitute.For<IDateTimeProvider>();
            _datetimeProviderMock.UtcNow.Returns(UtcNow);
            _pricingService = new PricingService();

            _handler = new ReserveBookingCommandHandler(
                  _userRepositoryMock,
                  _apartmentRepositoryMock,
                  _bookingRepositoryMock,
                  _unitOfWorkMock,
                  _pricingService,
                  _datetimeProviderMock
                );
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserIsNull()
        {
            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            var result = await _handler.Handle(Command, default);

            result.Error.Should().Be(UserErrors.NotFound);
        }
        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenApartmentIsNull()
        {
            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(UsersData.Create());

            _apartmentRepositoryMock.GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            var result = await _handler.Handle(Command, default);

            result.Error.Should().Be(ApartmentErrors.NotFound);
        }
        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenApartmentIsBooked()
        {
            var apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);
            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(UsersData.Create());

            
            _apartmentRepositoryMock.GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock.IsOverlappingAsync(apartment, duration, default)
                .Returns(true);

            var result = await _handler.Handle(Command, default);

            result.Error.Should().Be(BookingErrors.Overlap);
        }
        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
        {
            var user = UsersData.Create();
            
            var apartment = ApartmentData.Create();
            
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);
            
            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);


            _apartmentRepositoryMock.GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock.IsOverlappingAsync(apartment, duration, default)
                .Returns(false);

            _unitOfWorkMock.SaveChangesAsync(default)
                .ThrowsAsync(new ConcurrencyException("Concurrency", new Exception()));

            var result = await _handler.Handle(Command, default);

            result.Error.Should().Be(BookingErrors.Overlap);
        }
        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenBookingIsReserved()
        {
            var user = UsersData.Create();

            var apartment = ApartmentData.Create();

            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);


            _apartmentRepositoryMock.GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock.IsOverlappingAsync(apartment, duration, default)
                .Returns(false);


            var result = await _handler.Handle(Command, default);

            result.IsSuccess.Should().BeTrue();
        }

    }
}
