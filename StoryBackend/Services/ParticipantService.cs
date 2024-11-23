using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace StoryBackend.Services;

public class ParticipantService(StoryDbContext storyDbContext) : IParticipantService
{
    public async Task<GetParticipantDto> CreateParticipant(CreateParticipantDto createParticipantDto)
    {
        Participant participant = createParticipantDto.Adapt<Participant>();

        await storyDbContext.Participants.AddAsync(participant);
        await storyDbContext.SaveChangesAsync();

        return participant.Adapt<GetParticipantDto>();
    }

    public async Task<IEnumerable<GetParticipantDto>> GetParticipants(Guid storyId, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
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

    public async Task<bool> UserIsStoryParticipant(Guid userId, Guid storyId)
    {
        Participant? participant = await storyDbContext.Participants.FirstOrDefaultAsync(p => p.StoryId.Equals(storyId) && p.UserId.Equals(userId));
        return participant is not null;
    }
}
