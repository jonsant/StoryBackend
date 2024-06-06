using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBackend.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    //public IEnumerable<Story>? Stories { get; set; } = Enumerable.Empty<Story>();
    public DateTimeOffset Created { get; set; }
}
