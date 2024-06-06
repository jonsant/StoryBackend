namespace StoryBackend.Models
{
    public class LobbyMessage
    {
        public Guid LobbyMessageId { get; set; }
        public Guid StoryId { get; set; }
        //public Story? Story { get; set; }
        public Guid UserId { get; set; }
        //public User? User { get; set; }
        public DateTimeOffset Created {  get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
