using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.CommandsAndQueries
{
    public class InviteeCommandsAndQueries
    {
        public static async Task<IEnumerable<GetInviteeDto>> HandleGetStoryInvites(IInviteeService inviteeService, ClaimsPrincipal user) => await inviteeService.GetStoryInvites(user);
        public static async Task<GetParticipantDto?> HandleAcceptInvite(IInviteeService inviteeService, AcceptInviteDto acceptInviteDto, ClaimsPrincipal user) => await inviteeService.AcceptInvite(acceptInviteDto, user);

    }
}
