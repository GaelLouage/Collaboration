using Collaboration.Dtos;
using Collaboration.Entities;
using Collaboration.Extensions;
using Collaboration.Mappers;
using Collaboration.Repositories.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace Collaboration.Endpoints
{
    public static class UserEndpoints
    {
        public static void RegisterUserEndPoints(this WebApplication app)
        {
            app.GetUserByName();
            app.AddUser();
            app.UpdateUser();
            app.GetAllUsers();
            app.DeleteUser();
        }
        // get all users
        private static RouteHandlerBuilder GetAllUsers(this WebApplication app)
        {
            return app.MapGet("User/GetAllUsers", async (IMongoRepository<User> userRepository) =>
            {
                var users = (await userRepository.GetAllAsync()).Select(x => x.MapToUserDto());
                if(users is null)
                {
                    return Results.NotFound("No users found!");
                }
                return Results.Ok(users);
            });
        }
        // get a user by username
        private static RouteHandlerBuilder GetUserByName(this WebApplication app)
        {
            return app.MapGet("User/{name}", async (string name, IMongoRepository<User> userRepository) =>
            {
                var user = await userRepository.GetByUserNameAsync(name);

                if (user is null)
                {
                    return Results.NotFound($"No user with name {name} found!");
                }

                var userDto = user.MapToUserDto();
                return Results.Ok(userDto);
            });
        }
        // add a new user
        private static RouteHandlerBuilder AddUser(this WebApplication app)
        {
            return app.MapPost("User/AddUser", async (User user, IMongoRepository<User> userRepository) =>
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
            });
        }
        // update the user
        private static RouteHandlerBuilder UpdateUser(this WebApplication app)
        {
            return app.MapPatch("User/{name}", async (string name,[FromBody] User userModel, IMongoRepository<User> userRepository) =>
            {
                var user = await userRepository.GetByUserNameAsync(name);
                if(user is null)
                {
                    return Results.NotFound($"No user with name {name} found!");
                }
                // update the user in the database
                user.Email = userModel.Email;
                user.Password = userModel.Password.HashToPassword();
                user.Name = userModel.Name;
                user.UpdatedAt = DateTime.UtcNow;
                await userRepository.UpdateAsync(user.Id,user);
                return Results.Ok(user.MapToUserDto());
            });
        }
        // delete a user
        private static RouteHandlerBuilder DeleteUser(this WebApplication app)
        {
            return app.MapDelete("User/DeleteUserByName/{name}", async (string name, IMongoRepository<User> userRepository) =>
            {
                var user = await userRepository.GetByUserNameAsync(name);
                if(user is null)
                {
                    return Results.BadRequest("User not found!");
                }
                await userRepository.DeleteAsync(user.Id);
                return Results.Ok("Succesfully deleted user.");
            });
        }
    }
}
