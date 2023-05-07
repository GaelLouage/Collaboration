using Collaboration.Dtos;
using Collaboration.Entities;

namespace Collaboration.Mappers
{
    public static class UserMapper
    {
        public static UserDto MapToUserDto(this User user)
        {
            return new UserDto 
            { 
                Name = user.Name,
                Email =user.Email,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };
        }
    }
}
