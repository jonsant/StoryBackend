namespace StoryBackend.Models.DTOs;

public class GetUserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
}

public record CreateUserDto(Guid UserId)
{
    public static CreateUserDto Instance(Guid UserId) => new(UserId);
}
