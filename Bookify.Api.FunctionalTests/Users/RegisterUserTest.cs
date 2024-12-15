using Bookify.Api.Controllers.Users;
using Bookify.Api.FunctionalTests.Infrastructer;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookify.Api.FunctionalTests.Users
{
    public class RegisterUserTest(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
    {
        [Fact]
        public async Task Register_ShouldReturnOK_WhenRequestIsValid() 
        {
            var request = new RegisterUserRequest("create@test.com", "first", "last", "12345");
            var response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "first", "last", "12345")]
        [InlineData("test.com", "first", "last", "12345")]
        [InlineData("@test.com", "first", "last", "12345")]
        [InlineData("test@test.com", "", "last", "12345")]
        [InlineData("test@test.com", "first", "", "12345")]
        [InlineData("test@test.com", "first", "last", "")]
        [InlineData("test@test.com", "first", "last", "1")]
        [InlineData("test@test.com", "first", "last", "12")]
        [InlineData("test@test.com", "first", "last", "123")]
        [InlineData("test@test.com", "first", "last", "1234")]
        public async Task RegisterUser_ShouldReturnBadRequest_WhenRequestIsInValid(
            string email,
            string firstName,
            string lastName,
            string password)
        {
            var request = new RegisterUserRequest(email,firstName,lastName,password);
            var response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
