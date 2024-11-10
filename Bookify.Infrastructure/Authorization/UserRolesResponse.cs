using Bookify.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Authorization
{
    public sealed class UserRolesResponse
    {
        public Guid Id { get; init; }
        public List<Role> Roles { get; init; }
    }
}
