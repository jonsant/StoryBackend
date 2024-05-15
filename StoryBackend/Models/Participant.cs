namespace StoryBackend.Models
{
    public class Participant
    {
        public Guid ParticipantId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid StoryId { get; set; }
        public Story? Story { get; set; }
        public int PlayerInOrder {  get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
