using Bookify.Api.Controllers.Users;
using Bookify.Api.FunctionalTests.Users;
using Bookify.Application.Users.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookify.Api.FunctionalTests.Infrastructer
{
    public class BaseFunctionalTest(FunctionalTestWebAppFactory factory) : IClassFixture<FunctionalTestWebAppFactory>
    {
        protected readonly HttpClient HttpClient = factory.CreateClient();

        protected async Task<string> GetAccessToken() 
        {
            HttpResponseMessage loginResponse = await HttpClient
                .PostAsJsonAsync("api/v1/users/login",
                new LogInUserRequest(
                      UsersData.RegisterTestUserRequest.Email,
                      UsersData.RegisterTestUserRequest.Password
                    )); 
            var accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();
            return accessTokenResponse!.AccessToken;
        }
    }
}
