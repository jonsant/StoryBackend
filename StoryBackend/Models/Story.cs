namespace StoryBackend.Models
{
    public class Story
    {
        public Guid StoryId { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Finished { get; set; }
        public string Status { get; set; } = "Created"; // Created, Active, Finished
        public IEnumerable<Participant>? Participants { get; set; } = Enumerable.Empty<Participant>();
        public IEnumerable<Invitee>? Invitees { get; set; } = Enumerable.Empty<Invitee>();
        public IEnumerable<LobbyMessage>? LobbyMessages { get; set; } = Enumerable.Empty<LobbyMessage>();
        public IEnumerable<StoryEntry>? StoryEntries { get; set;} = Enumerable.Empty<StoryEntry>();
        public int? CurrentPlayerInOrder { get; set; }
        public string? FinalStory { get; set; }
    }
}
