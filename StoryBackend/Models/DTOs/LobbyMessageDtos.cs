namespace StoryBackend.Models.DTOs;

    public class CreateLobbyMessageDto
    {
        public Guid? StoryId { get; set; }
        public string? Message { get; set; }
    }

    public record GetLobbyMessageDto { 
        public Guid? LobbyMessageId { get; set; }
        public Guid? StoryId { get; set; }
        public Guid? UserId { get; set; }
        public string? Username { get; set; }
        public string? Message { get; set; }
        public DateTimeOffset? Created { get; set; }

        public static GetLobbyMessageDto Instance(Guid? lobbyMessageId, Guid? storyId, Guid? userId, string? username, string? message, DateTimeOffset? created)
        {
            return new()
            {
                LobbyMessageId = lobbyMessageId,
                StoryId = storyId,
                UserId = userId,
                Username = username,
                Message = message,
                Created = created
            };
        }
    }
