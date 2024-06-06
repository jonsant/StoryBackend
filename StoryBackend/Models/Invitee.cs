namespace StoryBackend.Models
{
    public class Invitee
    {
        public Guid InviteeId { get; set; }
        public Guid UserId { get; set; }
        //public User? User { get; set; }
        public Guid StoryId { get; set; }
        //public Story? Story { get; set; }
        public DateTimeOffset Created { get; set; }

        public static Invitee Instance(Guid userId, Guid storyId)
        {
            return new Invitee()
            {
                UserId = userId,
                StoryId = storyId,
                Created = DateTimeOffset.Now
            };
        }
    }
}
