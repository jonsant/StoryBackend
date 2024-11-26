using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IEmailWhitelistService
{
    public Task<bool> EmailExists(string email);
    public Task<AddEmailDto> AddEmail(AddEmailDto email);
}
