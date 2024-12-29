using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using StoryBackend.SignalR;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace StoryBackend.Services;

public class StoryService(StoryDbContext storyDbContext,
    IParticipantService participantService,
    IAuthManagementService authManagementService,
    //UserManager<IdentityUser> userManager,
    ICommonService commonService,
    IHubContext<StoryHub> storyHubContext,
    IHubContext<UserHub> userHubContext,
    IPushNotificationService pushNotificationService
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
        Guid? userId = await authManagementService.GetUserId(claimsPrincipal);
        if (userId is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(Guid.Parse(storyId)));
        if (story is null) return null;

        IEnumerable<Guid> inviteeUserIds = await storyDbContext.Invitees.Where(i => i.StoryId.Equals(story.StoryId)).Select(i => i.UserId).ToListAsync();
        //IEnumerable<Guid> participantUserIds = await storyDbContext.Participants.Where(p => !p.UserId.Equals(story.CreatorUserId) && p.StoryId.Equals(story.StoryId)).Select(p => p.UserId).ToListAsync();
        List<Guid> allParticipantUserIds = (await participantService.GetStoryParticipantIds(story.StoryId)).ToList();

        GetStoryDto storyDto = story.Adapt<GetStoryDto>();
        storyDto.Invitees = await commonService.GetUsernamesList(inviteeUserIds);
        //storyDto.Participants = await commonService.GetUsernamesList(participantUserIds);
        storyDto.Participants = await commonService.GetUsernamesList(allParticipantUserIds);
        storyDto.CreatorUsername = await commonService.GetUsernameById(story.CreatorUserId) ?? "";
        storyDto.NumberOfEntries = await storyDbContext.StoryEntries.CountAsync(s => s.StoryId.Equals(story.StoryId));

        // If story is active, include username of current player
        if (story.Status == "Active" && story.CurrentPlayerId is not null)
        {
            storyDto.CurrentPlayerUsername = await commonService.GetUsernameById(story.CurrentPlayerId.Value);
        }

        // If requesting user is current player in game, include the sentence to finish in the response
        if (story.CurrentPlayerId is not null && story.CurrentPlayerId.Equals(userId))
        {
            List<StoryEntry> allEntries = await storyDbContext.StoryEntries.
                Where(e => e.StoryId.Equals(story.StoryId)).ToListAsync();
            allEntries = allEntries.OrderByDescending(e => e.Created).ToList();
            StoryEntry? latestEntry = allEntries.FirstOrDefault();
            storyDto.SentenceToFinish = latestEntry is not null ? latestEntry.Second : null;
        }

        if (story.Finished is not null)
        {
            storyDto.FinalStoryEntries = await GetFinalStory(story);
        }
        return storyDto;
    }

    public async Task<IEnumerable<GetStoryDto>> GetStoriesByUserId(ClaimsPrincipal claimsPrincipal)
    {
        Guid? userId = await authManagementService.GetUserId(claimsPrincipal);
        if (userId is null) return null;

        List<GetStoryDto> stories = await storyDbContext.Stories.Where(s => s.CreatorUserId.Equals(userId)).Select(s => s.Adapt<GetStoryDto>()).ToListAsync();
        stories = stories.OrderByDescending(s => s.Created).ToList();
        return stories;
    }

    public async Task<IEnumerable<GetStoryDto>> GetParticipantStoriesByUserId(ClaimsPrincipal claimsPrincipal)
    {
        Guid? userId = await authManagementService.GetUserId(claimsPrincipal);
        if (userId is null) return null;

        List<Guid> participantEntries = await storyDbContext.Participants.Where(p => p.UserId.Equals(userId)).Select(pa => pa.StoryId).ToListAsync();

        List<GetStoryDto> stories = await storyDbContext.Stories.Where(s => !s.CreatorUserId.Equals(userId) && participantEntries.Contains(s.StoryId)).Select(s => s.Adapt<GetStoryDto>()).ToListAsync();

        List<Guid> creatorUserIds = stories.Select(s => s.CreatorUserId).ToList();
        Dictionary<Guid, string> creatorIdNameDict = (await commonService.GetUserIdUsernameDict(creatorUserIds)).ToDictionary();
        
        foreach (GetStoryDto story in stories)
        {
            story.CreatorUsername = creatorIdNameDict.First(c => c.Key.Equals(story.CreatorUserId)).Value;
        }

        stories = stories.OrderByDescending(s => s.Created).ToList();
        return stories;
    }

    public async Task<GetStoryDto> CreateStory(CreateStoryDto createStoryDto, ClaimsPrincipal claimsPrincipal)
    {
        Guid? id = await authManagementService.GetUserId(claimsPrincipal);
        if (id is null) return null;

        string? username = await commonService.GetUsernameById(id.Value);
        if (username is null) return null;

        Story newStory = Story.Instance(createStoryDto.StoryName, id.Value, "Created");
        await storyDbContext.Stories.AddAsync(newStory);
        await storyDbContext.SaveChangesAsync();

        CreateParticipantDto? createParticipant = CreateParticipantDto.Instance(newStory.StoryId, id.Value, DateTimeOffset.UtcNow);
        GetParticipantDto? createdParticipant = await participantService.CreateParticipant(createParticipant);

        foreach (string invitee in createStoryDto.Invitees)
        {
            User? userToInvite = await storyDbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(invitee.ToLower()));
            if (userToInvite is null) continue;

            Invitee newInvitee = Invitee.Instance(userToInvite.UserId, newStory.StoryId);
            await storyDbContext.Invitees.AddAsync(newInvitee);
            await storyDbContext.SaveChangesAsync();
            GetInviteeDto dto = GetInviteeDto.Instance(newInvitee.InviteeId, newStory.StoryName, newStory.StoryId, username, newInvitee.Created);
            await userHubContext.Clients.User(userToInvite.UserId.ToString()).SendAsync("NewInvite", dto);
            await pushNotificationService.SendNotification(PushNotification.Instance("New invite!", $"{username} invited you to join {newStory.StoryName}.", userToInvite.UserId));

        }
        GetStoryDto getStoryDto = newStory.Adapt<GetStoryDto>();
        getStoryDto.Invitees = createStoryDto.Invitees;
        return getStoryDto;
    }

    public async Task<StartStoryDto> StartStory(StartStoryDto startStoryDto, ClaimsPrincipal claimsPrincipal)
    {
        Guid? id = await authManagementService.GetUserId(claimsPrincipal);
        if (id is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(startStoryDto.StoryId));
        if (story is null) return null;

        if (!id.Value.Equals(story.CreatorUserId) || !story.Status.ToLower().Equals("created")) return null;

        story.Status = "Active";
        story.CurrentPlayerInOrder = 0;
        story.CurrentPlayerId = id.Value;

        if (await storyDbContext.SaveChangesAsync() > 0)
        {
            GetStoryDto? getStoryDto = await GetStoryById(story.StoryId.ToString(), claimsPrincipal);
            await storyHubContext.Clients.Group(story.StoryId.ToString()).SendAsync("StoryChanged", getStoryDto);
        }
        return startStoryDto;
    }

    public async Task<CreateEntryDto?> CreateEntry(CreateEntryDto createEntryDto, ClaimsPrincipal claimsPrincipal)
    {
        Guid? id = await authManagementService.GetUserId(claimsPrincipal);
        if (id is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(createEntryDto.StoryId));
        if (story is null) return null;

        if (!story.CurrentPlayerId.Equals(id)) return null;

        StoryEntry? storyEntry = StoryEntry.Instance(story.StoryId, id.Value, createEntryDto.First, createEntryDto.Second);
        await storyDbContext.StoryEntries.AddAsync(storyEntry);

        story.CurrentPlayerId = await GetNextPlayerId(story);
        if (await storyDbContext.SaveChangesAsync() > 0)
        {
            //GetStoryDto? getStoryDto = await GetStoryById(story.StoryId.ToString(), claimsPrincipal);
            await storyHubContext.Clients.Group(story.StoryId.ToString()).SendAsync("NewEntry", story.CurrentPlayerId);
            await pushNotificationService.SendNotification(PushNotification.Instance("Your turn!", $"It's your turn on {story.StoryName}", story.CurrentPlayerId.Value));
        }
        return createEntryDto;
    }

    public async Task<CreateEntryDto?> EndStory(CreateEntryDto createEntryDto, ClaimsPrincipal claimsPrincipal)
    {
        Guid? id = await authManagementService.GetUserId(claimsPrincipal);
        if (id is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(createEntryDto.StoryId));
        if (story is null) return null;

        if (!story.CurrentPlayerId.Equals(id) || !story.CreatorUserId.Equals(id)) return null;

        StoryEntry? storyEntry = StoryEntry.Instance(story.StoryId, id.Value, createEntryDto.First, null);
        await storyDbContext.StoryEntries.AddAsync(storyEntry);

        story.Status = "Finished";
        story.Finished = DateTimeOffset.UtcNow;
        story.CurrentPlayerId = null;

        if (await storyDbContext.SaveChangesAsync() > 0)
        {
            GetStoryDto? getStoryDto = await GetStoryById(story.StoryId.ToString(), claimsPrincipal);
            await storyHubContext.Clients.Group(story.StoryId.ToString()).SendAsync("StoryChanged", getStoryDto);
            await PushNotifyAllStoryParticipants(story.StoryId, $"Story finished", $"{story.StoryName} is finished!", story.CreatorUserId);
        }
        return createEntryDto;
    }

    private async Task<Guid> GetNextPlayerId(Story story)
    {
        List<Guid> allParticipants = (await participantService.GetStoryParticipantIds(story.StoryId)).ToList();
        int currentPlayerIndex = allParticipants.IndexOf(story.CurrentPlayerId!.Value);
        Guid nextPlayer = currentPlayerIndex == (allParticipants.Count - 1) ? 
            allParticipants[0] : 
            allParticipants[currentPlayerIndex + 1];
        return nextPlayer;
    }

    private async Task<IEnumerable<FinalStoryEntryDto>> GetFinalStory(Story story)
    {
        List<FinalStoryEntryDto> finalStoryEntries = Enumerable.Empty<FinalStoryEntryDto>().ToList();
        IEnumerable<StoryEntry> allEntries = await storyDbContext.StoryEntries.Where(e => e.StoryId.Equals(story.StoryId)).ToListAsync();
        allEntries = allEntries.OrderBy(e => e.Created);

        foreach (StoryEntry entry in allEntries)
        {
            string text = entry.First + " " + entry.Second;
            string username = await commonService.GetUsernameById(entry.UserId) ?? "Unknown";
            finalStoryEntries.Add(FinalStoryEntryDto.Instance(username, text));
        }
        return finalStoryEntries;
    }

    private async Task PushNotifyAllStoryParticipants(Guid storyId, string title, string message, Guid? excludedUserId = null)
    {
        IEnumerable<Guid> participantIds = await participantService.GetStoryParticipantIds(storyId);

        await Parallel.ForEachAsync(participantIds, async (participantId, c) => {
            if (excludedUserId is not null && participantId.Equals(excludedUserId)) return;
            await pushNotificationService.SendNotification(PushNotification.Instance(title, message, participantId));
        });
    }
}
