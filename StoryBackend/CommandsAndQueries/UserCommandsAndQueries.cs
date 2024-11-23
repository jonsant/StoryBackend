using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.CommandsAndQueries;

public class UserCommandsAndQueries
{
    public static async Task<IEnumerable<GetUserDto>> HandleGetUsers(IUserService userService) => await userService.GetUsers();
    public static async Task<GetUserDto?> HandleGetUserById(IUserService userService, Guid GlobalUserId) => await userService.GetUserById(GlobalUserId);
    public static async Task<IEnumerable<GetUserDto>> HandleGetUserByName(IUserService userService, string Username, ClaimsPrincipal user) => await userService.GetUserByName(Username, user);
    public static async Task<GetUserDto> HandleCreateUser(IUserService userService, CreateUserDto createUserDto) => await userService.CreateUser(createUserDto);
    public static async Task<GetUserDto?> HandleChangeUsername(IUserService userService, NewUsernameDto newUsernameDto, ClaimsPrincipal principal) => await userService.ChangeUsername(newUsernameDto, principal);
    public static async Task<bool> HandleUsernameAvailable(IUserService userService, string Username, ClaimsPrincipal user) => await userService.UsernameAvailable(Username, user);
}
