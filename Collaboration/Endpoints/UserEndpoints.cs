using Collaboration.Dtos;
using Collaboration.Entities;
using Collaboration.Extensions;
using Collaboration.Mappers;
using Collaboration.Repositories.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Collaboration.Repositories.Classes;
using Collaboration.Constants;
using System.Collections;

namespace Collaboration.Endpoints
{
    public static class UserEndpoints
    {
      
        public static void RegisterUserEndPoints(this WebApplication app)
        {
            
            app.MapGet("/GetUserByName/{name}", GetUserByName).WithDisplayName("GetUserByName");
            app.MapPost("/AddUser", AddUser).WithDisplayName("AddUser");
            app.MapPatch("/UpdateUser/{name}", UpdateUser).WithDisplayName("UpdateUser");
            app.MapGet("/GetAllUsers", GetAllUsers).WithDisplayName("GetAllUsers");
            app.MapDelete("/DeleteUser/{name}", DeleteUser).WithDisplayName("DeleteUser");
        }
        // get all users
        private static async Task<IResult> GetAllUsers([FromServices] MongoRepository<User> userRepository)
        {
            var users = (await userRepository.GetAllAsync()).Select(x => x.MapToUserDto());
            if (users is null)
            {
                return Results.NotFound("No users found!");
            }
            return Results.Ok(users);
        }
        // get a user by username
        private static async Task<IResult> GetUserByName([FromQuery] string name, [FromServices] IMongoRepository<User> userRepository)
        {
            var user = await userRepository.GetByUserNameAsync(name);

            if (user is null)
            {
                return Results.NotFound($"No user with name {name} found!");
            }

            var userDto = user.MapToUserDto();
            return Results.Ok(userDto);
        }
        // add a new user
        private static async Task<IResult> AddUser(User user, [FromServices] MongoRepository<User> userRepository)
        {
            user.UpdatedAt = DateTime.UtcNow;
            user.CreatedAt = DateTime.UtcNow;
            user.Password = user.Password.HashToPassword();
            await userRepository.InsertAsync(user);
            var userCreated = await userRepository.GetByUserNameAsync(user.Name);
            if (userCreated is null)
            {
                return Results.BadRequest($"Failed to create user!");
            }
            return Results.Ok(userCreated);
        }
        // update the user
        private static async Task<IResult> UpdateUser( string name, User userModel, [FromServices] MongoRepository<User> userRepository)
        {
            var user = await userRepository.GetByUserNameAsync(name);
            if (user is null)
            {
                return Results.NotFound($"No user with name {name} found!");
            }
            // update the user in the database
            user.Email = userModel.Email;
            user.Password = userModel.Password.HashToPassword();
            user.Name = userModel.Name;
            user.UpdatedAt = DateTime.UtcNow;
            await userRepository.UpdateAsync(user.Id, user);
            return Results.Ok(user.MapToUserDto());
        }
        // delete a user
        private static async Task<IResult> DeleteUser([FromBody] string name, [FromServices] MongoRepository<User> userRepository)
        {
            var user = await userRepository.GetByUserNameAsync(name);
            if (user is null)
            {
                return Results.BadRequest("User not found!");
            }
            await userRepository.DeleteAsync(user.Id);
            return Results.Ok("Succesfully deleted user.");
        }
    }
}
