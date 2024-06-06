using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IUserService
{
    public Task<IEnumerable<GetUserDto>> GetUsers();
    public Task<GetUserDto?> GetUserById(Guid globalUserId);
    public Task<IEnumerable<GetUserDto>> GetUserByName(string username, ClaimsPrincipal user);
    public Task<GetUserDto> CreateUser(CreateUserDto createUserDto);
}
