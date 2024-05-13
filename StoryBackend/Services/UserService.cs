using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;

namespace StoryBackend.Services;

public class UserService(StoryDbContext storyDbContext) : IUserService
{
    public async Task<GetUserDto> CreateUser(CreateUserDto createUserDto)
    {
        IEnumerable<User> allUsers = await storyDbContext.Users.ToListAsync();
        User? userExists = allUsers.FirstOrDefault(u => u.GlobalUserId.Equals(createUserDto.GlobalUserId));
        if (userExists is not null) return userExists.Adapt<GetUserDto>();

        User user = createUserDto.Adapt<User>();
        bool unique = false;

        while (!unique)
        {
            user.UserName = "story-user-" + Guid.NewGuid().ToString().Split('-')[0];
            unique = allUsers.FirstOrDefault(u => u.UserName.Equals(user.UserName)) is null;
        }

        storyDbContext.Users.Add(user);
        await storyDbContext.SaveChangesAsync();
        return user.Adapt<GetUserDto>();
    }

    public async Task<IEnumerable<GetUserDto>> GetUsers()
    {
        IEnumerable<GetUserDto> users = await storyDbContext.Users.Select(u => u.Adapt<GetUserDto>()).ToListAsync();
        return users;
    }

    public async Task<GetUserDto?> GetUserById(Guid globalUserId)
    {
        User? user = await storyDbContext.Users.FirstOrDefaultAsync(u => u.GlobalUserId == globalUserId);
        if (user is null) return null;

        return user.Adapt<GetUserDto>();
    }
}
