namespace StoryBackend.Models
{
    public class StoryEntry
    {
        public Guid StoryEntryId { get; set; }
        public Guid StoryId { get; set; }
        //public Story? Story { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
