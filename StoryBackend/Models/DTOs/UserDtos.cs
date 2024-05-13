namespace StoryBackend.Models.DTOs;

public class GetUserDto
{
    public Guid UserId { get; set; }
    public Guid GlobalUserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}

public record CreateUserDto(Guid GlobalUserId);
