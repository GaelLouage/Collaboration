using Collaboration.Entities;
using Collaboration.Enums;
using Collaboration.Extensions;
using Collaboration.Helpers;
using Collaboration.Repositories.Classes;
using Collaboration.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;

namespace Collaboration.Endpoints
{
    public static class LoginEndpoint
    {
        public static void RegisterLoginEndPoints(this WebApplication app)
        {
            app.MapPost("/Login/{userName}/{password}", Login).WithDisplayName("Login");

            app.MapPost("/Logout", Logout).WithDisplayName("Logout");
        }

        private static async Task<IResult> Login( [FromQuery] string userName, [FromQuery] string password, [FromServices] MongoRepository<User> userRepository)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Results.BadRequest("Username and password are required.");
            }

            User user = await userRepository.GetByUserNameAsync(userName);

            if (user == null || !HashPassword.VerifyPassword(password, user.Password))
            {
                return Results.Unauthorized();
            }

            var token = JwtTokenGenerator.GenerateToken(user);

            var response = new
            {
                token = token,
                user = new
                {
                    id = user.Id,
                    username = user.Name,
                    email = user.Email,
                    roles = user.Role
                }
            };

            return Results.Ok(response);
        }

        public static async Task<IResult> Logout(string authHeader, MongoRepository<BlacklistToken> blacklistedTokens)
        {
            // Extract the token from the authorization header
            string token = JwtTokenGenerator.ExtractTokenFromAuthHeader(authHeader);

            // Add the token to the blacklist
            await blacklistedTokens.InsertAsync(new BlacklistToken { Token = token });

            // Return a 204 No Content response
            return Results.NoContent();
        }
    }
}
