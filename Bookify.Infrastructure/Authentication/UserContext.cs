using Bookify.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Authentication
{
    internal sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string IdentityId => _httpContextAccessor.HttpContext?.User.GetIdentityId()
            ?? throw new ApplicationException("User context is unavailable");

        public Guid UserId => _httpContextAccessor.HttpContext?.User.GetUserId()
             ?? throw new ApplicationException("User context is unavailable");
    }
}
