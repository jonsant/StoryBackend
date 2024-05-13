using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;

namespace StoryBackend.CommandsAndQueries;

public class UserCommandsAndQueries
{
    public static async Task<IEnumerable<GetUserDto>> HandleGetUsers(IUserService userService) => await userService.GetUsers();
    public static async Task<GetUserDto?> HandleGetUserById(IUserService userService, Guid GlobalUserId) => await userService.GetUserById(GlobalUserId);
    public static async Task<GetUserDto> HandleCreateUser(IUserService userService, CreateUserDto createUserDto) => await userService.CreateUser(createUserDto);
}
