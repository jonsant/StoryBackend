using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace StoryBackend.Services;

public class InviteeService(StoryDbContext storyDbContext, IParticipantService participantService,
    IAuthManagementService authManagementService) : IInviteeService
{
    public async Task<GetInviteeDto> CreateInvitee(CreateInviteeDto createInviteeDto, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
        //Invitee invitee = createInviteeDto.Adapt<Invitee>();

        //await storyDbContext.Invitees.AddAsync(invitee);
        //await storyDbContext.SaveChangesAsync();
        //return await Task.FromResult(invitee.Adapt<GetInviteeDto>());
    }

    public async Task<GetParticipantDto?> AcceptInvite(AcceptInviteDto acceptInviteDto, ClaimsPrincipal user)
    {
        Guid? id = await authManagementService.GetUserId(user);
        if (id is null) return null;

        if (acceptInviteDto.InviteeId is null) return null;
        //Guid? id = Guid.Parse("EECB35DB-AD6E-4101-8369-55DB6F7555CE");

        Invitee? invite = await storyDbContext.Invitees.FirstOrDefaultAsync(i => i.InviteeId.Equals(acceptInviteDto.InviteeId));
        if (invite is null) return null;
        if (!invite.UserId.Equals(id)) return null;

        CreateParticipantDto? createParticipant = CreateParticipantDto.Instance(invite.StoryId, invite.UserId, DateTimeOffset.Now);
        GetParticipantDto? createdParticipant = await participantService.CreateParticipant(createParticipant);

        if (createdParticipant is not null)
        {
            await DeleteInvite(invite.InviteeId);
        }

        return createdParticipant;
    }

    public async Task<IEnumerable<GetInviteeDto>> GetStoryInvites(ClaimsPrincipal user)
    {
        Guid? id = await authManagementService.GetUserId(user);
        if (id is null) return null;
        //Guid? id = Guid.Parse("EECB35DB-AD6E-4101-8369-55DB6F7555CE");

        List<Invitee> invites = await storyDbContext.Invitees.Where(i => i.UserId.Equals(id)).ToListAsync();
        List<GetInviteeDto> inviteeDtos = new List<GetInviteeDto>();

        foreach (var invitee in invites)
        {
            Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(invitee.StoryId));
            if (story is null) continue;
            User? creatorUser = await storyDbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(story.CreatorUserId));
            if (creatorUser is null) continue;

            inviteeDtos.Add(GetInviteeDto.Instance(invitee.InviteeId, story.StoryName, invitee.StoryId, creatorUser.Username, invitee.Created));
        }

        inviteeDtos = inviteeDtos.OrderByDescending(i => i.InvitedDate).ToList();

        return inviteeDtos;
    }

    public async Task<Guid?> DeleteInvite(Guid inviteeId)
    {
        Invitee? invite = await storyDbContext.Invitees.FirstOrDefaultAsync(i => i.InviteeId.Equals(inviteeId));
        if (invite is null) return null;

        storyDbContext.Invitees.Remove(invite);
        await storyDbContext.SaveChangesAsync();

        return inviteeId;
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
