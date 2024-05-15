namespace StoryBackend.Models
{
    public class Invitee
    {
        public Guid InviteeId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid StoryId { get; set; }
        public Story? Story { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
