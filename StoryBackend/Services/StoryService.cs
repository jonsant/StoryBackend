using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace StoryBackend.Services;

public class StoryService(StoryDbContext storyDbContext) : IStoryService
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

    public async Task<GetStoryDto> GetStoryById(string storyId, ClaimsPrincipal user)
   {
        var id = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (id is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(Guid.Parse(storyId)));
        if (story is null) return null;

        IEnumerable<Guid> inviteeUserIds = await storyDbContext.Invitees.Where(i => i.StoryId.Equals(story.StoryId)).Select(i => i.UserId).ToListAsync();
        IEnumerable<Guid> participantUserIds = await storyDbContext.Participants.Where(p => p.StoryId.Equals(story.StoryId)).Select(p => p.UserId).ToListAsync();
        
        GetStoryDto storyDto = story.Adapt<GetStoryDto>();
        storyDto.Invitees = await GetUsernamesList(inviteeUserIds);
        storyDto.Participants = await GetUsernamesList(participantUserIds);

        return await Task.FromResult(storyDto);
    }

    private async Task<IEnumerable<string>> GetUsernamesList(IEnumerable<Guid> userIds)
    {
        List<string> usernames = new List<string>();
        foreach (Guid id in userIds)
        {
            User? user = await storyDbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(id));
            if (user is null) continue;
            usernames.Add(user.Username);
        }
        return usernames;
    }

    public async Task<IEnumerable<GetStoryDto>> GetStoriesByUserId(ClaimsPrincipal user)
    {
        var id = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (id is null) return null;

        List<GetStoryDto> stories = await storyDbContext.Stories.Where(s => s.CreatorUserId.Equals(Guid.Parse(id))).Select(s => s.Adapt<GetStoryDto>()).ToListAsync();
        return stories;
    }

    public async Task<GetStoryDto> CreateStory(CreateStoryDto createStoryDto, ClaimsPrincipal user)
    {
        var id = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (id is null) return null;
        Story newStory = Story.Instance(createStoryDto.StoryName, Guid.Parse(id), "Created");
        await storyDbContext.Stories.AddAsync(newStory);
        await storyDbContext.SaveChangesAsync();

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
