using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace StoryBackend.Services;

public class StoryService(StoryDbContext storyDbContext,
    IParticipantService participantService,
    IAuthManagementService authManagementService,
    UserManager<IdentityUser> userManager
    ) : IStoryService
{
    public async Task<GetWeatherForecastDto> CreateForecastTest(CreateWeatherForecast createWeatherForecastDto)
    {
        WeatherForecast forecast = createWeatherForecastDto.Adapt<WeatherForecast>();

        await storyDbContext.WeatherForecasts.AddAsync(forecast);
        await storyDbContext.SaveChangesAsync();
        return await Task.FromResult(forecast.Adapt<GetWeatherForecastDto>());
    }

    public async Task<IEnumerable<GetWeatherForecastDto>> GetForecastBackendTest()
    {
        IEnumerable<GetWeatherForecastDto> forecasts = await storyDbContext.WeatherForecasts.Select(f => f.Adapt<GetWeatherForecastDto>()).ToListAsync();
        return await Task.FromResult(forecasts);
    }

    public async Task<GetStoryDto> GetStoryById(string storyId, ClaimsPrincipal claimsPrincipal)
   {
        //if ((await authManagementService.GetUserId(claimsPrincipal)) is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(Guid.Parse(storyId)));
        if (story is null) return null;

        IEnumerable<Guid> inviteeUserIds = await storyDbContext.Invitees.Where(i => i.StoryId.Equals(story.StoryId)).Select(i => i.UserId).ToListAsync();
        IEnumerable<Guid> participantUserIds = await storyDbContext.Participants.Where(p => !p.UserId.Equals(story.CreatorUserId) && p.StoryId.Equals(story.StoryId)).Select(p => p.UserId).ToListAsync();
        
        GetStoryDto storyDto = story.Adapt<GetStoryDto>();
        storyDto.Invitees = await GetUsernamesList(inviteeUserIds);
        storyDto.Participants = await GetUsernamesList(participantUserIds);
        storyDto.CreatorUsername = await GetUsernameById(story.CreatorUserId) ?? "";

        return await Task.FromResult(storyDto);
    }

    private async Task<IEnumerable<string>> GetUsernamesList(IEnumerable<Guid> userIds)
    {
        List<string> usernames = new List<string>();
        foreach (Guid id in userIds)
        {
            string? username = await GetUsernameById(id);
            if  (username is not null) usernames.Add(username);
        }
        return usernames;
    }

    private async Task<string?> GetUsernameById(Guid userId)
    {
        User? user = await storyDbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
        return user is null ? null : user.Username;
    }


    public async Task<IEnumerable<GetStoryDto>> GetStoriesByUserId(ClaimsPrincipal claimsPrincipal)
    {
        Guid? userId = await authManagementService.GetUserId(claimsPrincipal);
        if (userId is null) return null;

        List<GetStoryDto> stories = await storyDbContext.Stories.Where(s => s.CreatorUserId.Equals(userId)).Select(s => s.Adapt<GetStoryDto>()).ToListAsync();
        return stories;
    }

    public async Task<IEnumerable<GetStoryDto>> GetParticipantStoriesByUserId(ClaimsPrincipal claimsPrincipal)
    {
        Guid? userId = await authManagementService.GetUserId(claimsPrincipal);
        if (userId is null) return null;

        List<Guid> participantEntries = await storyDbContext.Participants.Where(p => p.UserId.Equals(userId)).Select(pa => pa.StoryId).ToListAsync();

        List<GetStoryDto> stories = await storyDbContext.Stories.Where(s => !s.CreatorUserId.Equals(userId) && participantEntries.Contains(s.StoryId)).Select(s => s.Adapt<GetStoryDto>()).ToListAsync();
        return stories;
    }

    public async Task<GetStoryDto> CreateStory(CreateStoryDto createStoryDto, ClaimsPrincipal claimsPrincipal)
    {
        Guid? id = await authManagementService.GetUserId(claimsPrincipal);
        if (id is null) return null;

        Story newStory = Story.Instance(createStoryDto.StoryName, id.Value, "Created");
        await storyDbContext.Stories.AddAsync(newStory);
        await storyDbContext.SaveChangesAsync();

        CreateParticipantDto? createParticipant = CreateParticipantDto.Instance(newStory.StoryId, id.Value, DateTimeOffset.Now);
        GetParticipantDto? createdParticipant = await participantService.CreateParticipant(createParticipant);

        foreach (string invitee in createStoryDto.Invitees)
        {
            User? userToInvite = await storyDbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(invitee.ToLower()));
            if (userToInvite is null) continue;

            Invitee newInvitee = Invitee.Instance(userToInvite.UserId, newStory.StoryId);
            await storyDbContext.Invitees.AddAsync(newInvitee);
            await storyDbContext.SaveChangesAsync();
        }
        GetStoryDto getStoryDto = newStory.Adapt<GetStoryDto>();
        getStoryDto.Invitees = createStoryDto.Invitees;
        return await Task.FromResult(getStoryDto);
    }
}
