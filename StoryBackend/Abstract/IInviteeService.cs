using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IInviteeService
{
    public Task<GetInviteeDto> CreateInvitee(CreateInviteeDto createInviteeDto, ClaimsPrincipal user);
    public Task<IEnumerable<GetInviteeDto>> GetStoryInvites(ClaimsPrincipal user);
    public Task<GetParticipantDto?> AcceptInvite(AcceptInviteDto acceptInviteDto, ClaimsPrincipal user);


}
