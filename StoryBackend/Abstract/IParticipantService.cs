using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IParticipantService
{
    public Task<GetParticipantDto> CreateParticipant(CreateParticipantDto createParticipantDto);
    public Task<IEnumerable<GetParticipantDto>> GetParticipants(Guid storyId, ClaimsPrincipal user);
}
