using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
//using StoryBackend.Migrations;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Services;

public class UserService(StoryDbContext storyDbContext, IAuthManagementService authManagementService) : IUserService
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
            user.Username = "user-" + Guid.NewGuid().ToString().Split('-')[0];
            unique = allUsers.FirstOrDefault(u => u.Username.Equals(user.Username)) is null;
        }
        user.Created = DateTimeOffset.UtcNow;
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
        Guid? id = await authManagementService.GetUserId(user);
        if (id is null) return Enumerable.Empty<GetUserDto>();

        List<User> users = await storyDbContext.Users.Where(u => u.Username.ToLower().Contains(username.ToLower())).ToListAsync();
        users = users.Where(u => !u.UserId.ToString().Equals(id.ToString())).ToList();
        return users.Select(u => u.Adapt<GetUserDto>());
    }

    public async Task<bool> UsernameAvailable(string username, ClaimsPrincipal claimsPrincipal)
    {
        string? usernameExists = await storyDbContext.Users.Select(u => u.Username).FirstOrDefaultAsync(name => name.ToLower().Equals(username.ToLower()));
        return usernameExists is null;
    }

    public async Task<GetUserDto?> ChangeUsername(NewUsernameDto newUsernameDto, ClaimsPrincipal claimsPrincipal)
    {
        Guid? id = await authManagementService.GetUserId(claimsPrincipal);
        if (id is null) return null;

        string? usernameTaken = await storyDbContext.Users.Select(u => u.Username).FirstOrDefaultAsync(name => name.ToLower().Equals(newUsernameDto.NewUsername.ToLower()));
        if (usernameTaken is not null) return null;

        User? user = await storyDbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(id));
        if (user is null) return null;

        user.Username = newUsernameDto.NewUsername;
        await storyDbContext.SaveChangesAsync();
        return user.Adapt<GetUserDto>();
    }
}
