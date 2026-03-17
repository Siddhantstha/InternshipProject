using Application.Service;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace UserProject.UnitTest
{
    public class TokenTest
    {
        private readonly TokenService _service;

        public TokenTest()
        {
            var settings = new Dictionary<string, string?>
        {
            {"JwtSettings:Key", "THIS_IS_SUPER_SECRET_KEY_123456789"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"},
            {"JwtSettings:ExpiryMinutes", "60"}
        };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            _service = new TokenService(configuration);
        }

        [Fact]
        public void TokenGenerate_WhenCalled_ReturnsValidToken()
        {
            // Arrange
            var user = new Domain.Entities.User
            {
                Id = 1,
                Email = "test@email.com",
                Role = "Admin"
            };

            // Act
            var token = _service.TokenGenerate(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void TokenGenerate_ShouldContainCorrectClaims()
        {
            // Arrange
            var user = new Domain.Entities.User
            {
                Id = 5,
                Email = "admin@test.com",
                Role = "Admin"
            };

            var token = _service.TokenGenerate(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == user.Role);
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
        }

        [Fact]
        public void TokenGenerate_ShouldHaveExpiration()
        {
            var user = new Domain.Entities.User
            {
                Id = 10,
                Email = "user@test.com",
                Role = "Customer"
            };

            var token = _service.TokenGenerate(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
        }
    }
}
