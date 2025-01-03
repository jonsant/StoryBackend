﻿using Mapster;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using StoryBackend.SignalR;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace StoryBackend.Services;

public class InviteeService(StoryDbContext storyDbContext,
    IParticipantService participantService,
    IPushNotificationService pushNotificationService,
    IAuthManagementService authManagementService,
    ICommonService commonService,
    IHubContext<StoryHub> storyHubContext,
    IHubContext<UserHub> userHubContext) : IInviteeService
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

        Story? story = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId.Equals(invite.StoryId));
        if (story is null) return null;

        CreateParticipantDto? createParticipant = CreateParticipantDto.Instance(invite.StoryId, invite.UserId, DateTimeOffset.UtcNow);
        GetParticipantDto? createdParticipant = await participantService.CreateParticipant(createParticipant);

        if (createdParticipant is not null)
        {
            string? invitedUsername = await commonService.GetUsernameById(invite.UserId);
            await DeleteInvite(invite.InviteeId);
            await storyHubContext.Clients.Group(createdParticipant.StoryId.ToString()).SendAsync("InviteAccepted", invitedUsername ?? "A user");
            await pushNotificationService.SendNotification(PushNotification.Instance($"Invite accepted!", $"{invitedUsername} accepted your invite to {story.StoryName}!", story.CreatorUserId));
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
}
