using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Services;

public class EmailWhitelistService(StoryDbContext storyDbContext, IAuthManagementService authManagementService) : IEmailWhitelistService
{
    public async Task<bool> EmailExists(string email)
    {
        return await storyDbContext.EmailWhitelist.AnyAsync(e => e.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<AddEmailDto> AddEmail(AddEmailDto emailDto)
    {
        EmailWhitelist emailToAdd = EmailWhitelist.Instance(emailDto.Email);
        await storyDbContext.EmailWhitelist.AddAsync(emailToAdd);
        string res = "";
        if (await storyDbContext.SaveChangesAsync() > 0)
        {
            res = emailToAdd.Email;
        }
        emailDto.Email = res;
        return emailDto;
    }
}
