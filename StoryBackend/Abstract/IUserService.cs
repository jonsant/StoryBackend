using StoryBackend.Models.DTOs;

namespace StoryBackend.Abstract;

public interface IUserService
{
    public Task<IEnumerable<GetUserDto>> GetUsers();
    public Task<GetUserDto?> GetUserById(Guid globalUserId);
    public Task<GetUserDto> CreateUser(CreateUserDto createUserDto);
}
