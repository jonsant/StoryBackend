using System.ComponentModel.DataAnnotations;

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

public class UserRegistrationRequestDto
{
    //[Required]
    //public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class UserRegistrationResponseDto : AuthResult
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}

public class AuthResult
{
    public string Token { get; set; } = string.Empty;
    public bool Result { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}

public class UserLoginRequestDto
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class UserLoginResponseDto : AuthResult
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}

public class CreateRoleResponseDto
{
    public string Role { get; set; } = string.Empty;
    public string? Errors { get; set; }
}

public class CreateRoleRequestDto
{
    public string Role { get; set; } = string.Empty;
}
