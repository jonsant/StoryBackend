using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Services;

public class UserService(StoryDbContext storyDbContext) : IUserService
{
    public async Task<GetUserDto> CreateUser(CreateUserDto createUserDto)
    {
        IEnumerable<User> allUsers = await storyDbContext.Users.ToListAsync();
        User? userExists = allUsers.FirstOrDefault(u => u.UserId.Equals(createUserDto.UserId));
        if (userExists is not null) return userExists.Adapt<GetUserDto>();

        User user = createUserDto.Adapt<User>();
        bool unique = false;

        while (!unique)
        {
            user.Username = "story-user-" + Guid.NewGuid().ToString().Split('-')[0];
            unique = allUsers.FirstOrDefault(u => u.Username.Equals(user.Username)) is null;
        }
        user.Created = DateTimeOffset.Now;
        storyDbContext.Users.Add(user);
        await storyDbContext.SaveChangesAsync();
        return user.Adapt<GetUserDto>();
    }

    public async Task<IEnumerable<GetUserDto>> GetUsers()
    {
        IEnumerable<GetUserDto> users = await storyDbContext.Users.Select(u => u.Adapt<GetUserDto>()).ToListAsync();
        return users;
    }

    public async Task<GetUserDto?> GetUserById(Guid userId)
    {
        GetUserDto user = await CreateUser(CreateUserDto.Instance(userId));
        return user;
        //User? user = await storyDbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
        //if (user is null) {
        //    await 
        //}

        //return user.Adapt<GetUserDto>();
    }

    public async Task<IEnumerable<GetUserDto>> GetUserByName(string username, ClaimsPrincipal user)
    {
        var id = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (id is null) return Enumerable.Empty<GetUserDto>();

        List<User> users = await storyDbContext.Users.Where(u => u.Username.ToLower().Contains(username.ToLower())).ToListAsync();
        users = users.Where(u => !u.UserId.ToString().Equals(id)).ToList();
        return users.Select(u => u.Adapt<GetUserDto>());
    }
}
