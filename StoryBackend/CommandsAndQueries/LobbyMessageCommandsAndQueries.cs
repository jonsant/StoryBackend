using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.CommandsAndQueries
{
    public class LobbyMessageCommandsAndQueries
    {
        public static async Task<IEnumerable<GetLobbyMessageDto>> HandleGetLobbyMessagesByStoryId(ILobbyMessageService lobbyMessageService, string storyId, ClaimsPrincipal user) => await lobbyMessageService.GetLobbyMessagesByStoryId(storyId, user);
        public static async Task<GetLobbyMessageDto?> HandleCreateLobbyMessage(ILobbyMessageService lobbyMessageService, CreateLobbyMessageDto createLobbyMessageDto, ClaimsPrincipal user) => await lobbyMessageService.CreateLobbyMessage(createLobbyMessageDto, user);

    }
}
