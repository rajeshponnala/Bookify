using Bookify.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _isender;

        public UsersController(ISender isender)
        {
            _isender = isender;
        }

        [AllowAnonymous]
        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken=default)
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password);

            var result = await _isender.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);

        }

    }
}
