using Microsoft.AspNetCore.Authorization;
using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.CommandsAndQueries;

public class EmailWhitelistCommandsAndQueries
{
    public static async Task<AddEmailDto> HandleAddEmail(IEmailWhitelistService emailWhitelistService, AddEmailDto addEmailDto) => await emailWhitelistService.AddEmail(addEmailDto);
}
