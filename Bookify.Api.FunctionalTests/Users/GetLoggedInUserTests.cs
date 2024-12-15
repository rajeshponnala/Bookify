using Bookify.Api.FunctionalTests.Infrastructer;
using Bookify.Application.Users.GetLoggedInUser;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Bookify.Api.FunctionalTests.Users
{
    public class GetLoggedInUserTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
    {
        [Fact]
        public async Task Get_ShouldReturnUnAuthorized_WhenAccessTokenIsMissing() 
        {
            var response = await HttpClient.GetAsync("api/v1/users/me");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        //Fix: Test failing, need to fix
        [Fact]
        public async Task Get_ShouldReturnOK_WhenAccessTokenIsNotMissing()
        {
            var accessToken = await GetAccessToken();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                  JwtBearerDefaults.AuthenticationScheme,
                  accessToken
                );
            var user = await HttpClient.GetFromJsonAsync<UserResponse>("api/v1/users/me");
            user.Should().NotBeNull();
        }
    }
}
