using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface ILobbyMessageService
{
    public Task<GetLobbyMessageDto> CreateLobbyMessage(CreateLobbyMessageDto createLobbyMessageDto, ClaimsPrincipal user);
    public Task<IEnumerable<GetLobbyMessageDto>> GetLobbyMessagesByStoryId(string storyId, ClaimsPrincipal user);

    //public Task<GetLobbyMessageDto> CreateInfoLobbyMessage(CreateLobbyMessageDto infoLobbyMessageDto);


}
