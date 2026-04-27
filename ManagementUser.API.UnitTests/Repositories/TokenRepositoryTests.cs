using ManagementUser.API.Data;
using ManagementUser.API.Repositories.Implementation;
using ManagementUser.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ManagementUser.API.UnitTests.Repositories
{
    public class TokenRepositoryTests
    {
        private IConfiguration GetTestConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?> {
                {"Jwt:Key", "ThisIsASecretKeyForJwtTokenGeneration"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return configuration;
        }

        [Fact]
        public void TokenRepository_CreateJwtToken_ReturnsValidToken()
        {
            // Arrange
            var configuration = GetTestConfiguration();
            var repository = new TokenRepository(configuration);
            var user = new ApplicationUser
            {
                Email = "test@example.com"
            };
            var roles = new List<string> { "Admin", "User" };

            // Act
            var token = repository.CreateJwtToken(user, roles);

            // Assert
            token.Should().NotBeNullOrEmpty();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            jwtToken.Should().NotBeNull();
            jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value.Should().Be(user.Email);
            jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).Should().BeEquivalentTo(roles);
        }
    }
}