using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace StoryBackend.Services;

public class LobbyMessageService(StoryDbContext storyDbContext) : ILobbyMessageService
{
    public async Task<GetLobbyMessageDto> CreateLobbyMessage(CreateLobbyMessageDto createLobbyMessageDto, ClaimsPrincipal user)
    {
        var id = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (id is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(createLobbyMessageDto.StoryId));
        if (story is null) return null;

        Participant? participant = await storyDbContext.Participants.FirstOrDefaultAsync(p => p.StoryId.Equals(createLobbyMessageDto.StoryId)
         && p.UserId.Equals(Guid.Parse(id)));
        if (participant is null) return null;

        LobbyMessage lobbyMessage = createLobbyMessageDto.Adapt<LobbyMessage>();
        lobbyMessage.Created = DateTimeOffset.Now;
        lobbyMessage.UserId = Guid.Parse(id);
        await storyDbContext.LobbyMessages.AddAsync(lobbyMessage);
        await storyDbContext.SaveChangesAsync();
        return lobbyMessage.Adapt<GetLobbyMessageDto>();
    }

    public async Task<IEnumerable<GetLobbyMessageDto>> GetLobbyMessagesByStoryId(string storyId, ClaimsPrincipal user)
    {
        Guid storyIdGuid = Guid.Parse(storyId);
        var id = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (id is null) return null;

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(storyIdGuid));
        if (story is null) return null;

        Participant? participant = await storyDbContext.Participants.FirstOrDefaultAsync(p => p.UserId.Equals(Guid.Parse(id)) && p.StoryId.Equals(Guid.Parse(storyId)));
        if (participant is null) return null;

        //IEnumerable<Participant> storyParticipants = await storyDbContext.Participants.Where(p => p.StoryId.Equals(storyIdGuid)).ToListAsync();
        List<User> usernames = Enumerable.Empty<User>().ToList();
        IEnumerable<LobbyMessage> lobbyMessages = await storyDbContext.LobbyMessages.Where(l => l.StoryId.Equals(storyIdGuid)).ToListAsync();
        lobbyMessages.OrderBy(l => l.Created);
        IEnumerable<Guid> messageUsers = lobbyMessages.Select(l => l.UserId);

        foreach(Guid userId in messageUsers)
        {
            User? messageUser = await storyDbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
            if (messageUser is null) continue;
            usernames.Add(messageUser);
        }

        List<GetLobbyMessageDto> lobbyMessageDtos = Enumerable.Empty<GetLobbyMessageDto>().ToList();
        foreach (var item in lobbyMessages)
        {
            User? messageUser = usernames.FirstOrDefault(u => u.UserId.Equals(item.UserId));
            if (messageUser is null) continue;
            string username = messageUser.Username;

            GetLobbyMessageDto getLobbyMessageDto = GetLobbyMessageDto.Instance(
                item.LobbyMessageId,
                item.StoryId,
                item.UserId,
                username,
                item.Message,
                item.Created
                );
            lobbyMessageDtos.Add(getLobbyMessageDto);
        }
        return lobbyMessageDtos;
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
}
