namespace StoryBackend.Models
{
    public class StoryEntry
    {
        public Guid StoryEntryId { get; set; }
        public Guid StoryId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string First { get; set; } = string.Empty;
        public string? Second { get; set; } = string.Empty;

        public static StoryEntry Instance(Guid storyId, Guid userId, string first, string? second)
        {
            return new StoryEntry()
            {
                StoryId = storyId,
                UserId = userId,
                Created = DateTimeOffset.UtcNow,
                First = first,
                Second = second
            };
        } 
    }
}
