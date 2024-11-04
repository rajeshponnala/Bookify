﻿using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Authentication
{
    public sealed class JwtService : IJwtService
    {
        private static readonly Error AuthenticationFailed = new(
            "Keycloak.AuthenticationFailed",
            "Failed to acquire access token due to authentication failure");

        private readonly HttpClient _httpClient;
        private readonly KeycloakOptions _keycloakOptions;

        public JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
        {
            _httpClient = httpClient;
            _keycloakOptions = keycloakOptions.Value;
        }

        public async Task<Result<string>> GetAccessTokenAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var authReqParams = new KeyValuePair<string, string>[]
                {
                    new("client_id", _keycloakOptions.AdminClientId),
                    new("client_secret", _keycloakOptions.AdminClientSecret),
                    new("scope", "openid email"),
                    new("grant_type", "password"),
                    new("username", email),
                    new("password", password)
                };
                var authorizationRequestContent = new FormUrlEncodedContent(authReqParams);
                var response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);
                response.EnsureSuccessStatusCode();
                var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>();
                if(authorizationToken is null)
                {
                    return Result.Failure<string>(AuthenticationFailed);
                }
                return authorizationToken.AccessToken;
            }
            catch (HttpRequestException)
            {
                return  Result.Failure<string>(AuthenticationFailed);
            }
        }
    }
}
