using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBackend.Models;

public class EmailWhitelist
{
    public Guid EmailWhitelistId { get; set; }
    public string Email { get; set; } = string.Empty;
    public static EmailWhitelist Instance(string email) => new() { Email = email };

}
