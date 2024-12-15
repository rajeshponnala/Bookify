using Bookify.Api.Controllers.Users;
using Bookify.Api.FunctionalTests.Infrastructer;
using Bookify.Api.FunctionalTests.Users;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Bookify.Api.FunctionalTests.Users
{
    public class LoginUserTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
    {
        private const string Email = "login@test.com";
        private const string Password = "12345";

        [Fact]
        public async Task Login_ShouldReturnUnAuthorized_WhenUserDoesNotExist() 
        {
            var request = new LogInUserRequest(Email, Password);
            var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_ShouldReturnOK_WhenUserExist()
        {
            var request = new LogInUserRequest(
                UsersData.RegisterTestUserRequest.Email,
                UsersData.RegisterTestUserRequest.Password);
            var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
