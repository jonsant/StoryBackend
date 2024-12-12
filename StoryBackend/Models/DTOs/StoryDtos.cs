namespace StoryBackend.Models.DTOs;

public record CreateStoryDto(string StoryName, IEnumerable<string> Invitees)
{
    public static CreateStoryDto Instance(string storyName, IEnumerable<string> Invitees) => new CreateStoryDto(storyName, Invitees);
}

public record GetStoryDto()
{
    public Guid StoryId { get; set; }
    public string? StoryName { get; set; }
    public Guid CreatorUserId { get; set; }
    public string CreatorUsername { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Finished { get; set; }
    public string? Status { get; set; } // Created, Active, Finished
    public int? CurrentPlayerInOrder { get; set; }
    public Guid? CurrentPlayerId { get; set; }
    public string? CurrentPlayerUsername { get; set; }
    public string? FinalStory { get; set; }
    public IEnumerable<string>? Invitees { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string>? Participants { get; set; } = Enumerable.Empty<string>();
    public int? NumberOfEntries { get; set; }
    public string? SentenceToFinish { get; set; }
    public IEnumerable<FinalStoryEntryDto>? FinalStoryEntries { get; set; }
}

public record StartStoryDto(Guid StoryId)
{
    public static StartStoryDto Instance(Guid storyId) => new(storyId);
}

//public record GetStartStoryDto(Guid StoryId);