namespace StoryBackend.Models.DTOs
{
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
        public string? FinalStory { get; set; }
        public IEnumerable<string>? Invitees { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string>? Participants { get; set; } = Enumerable.Empty<string>();
    }
}
