using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IInviteeService
{
    public Task<GetInviteeDto> CreateInvitee(CreateInviteeDto createInviteeDto, ClaimsPrincipal user);

}
