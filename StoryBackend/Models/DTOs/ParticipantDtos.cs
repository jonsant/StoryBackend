namespace StoryBackend.Models.DTOs
{
    public class CreateParticipantDto
    {
        public Guid StoryId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset Created { get; set; }

        public static CreateParticipantDto Instance(Guid storyId, Guid userId, DateTimeOffset created) =>
            new() { StoryId = storyId, UserId = userId, Created = created };
    }

    public record GetParticipantDto { 
        public Guid ParticipantId { get; set; }
        public Guid StoryId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
