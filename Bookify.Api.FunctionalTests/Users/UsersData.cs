using Bookify.Api.Controllers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Api.FunctionalTests.Users
{
    public static class UsersData
    {
        public static RegisterUserRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "12345");

    }
}
