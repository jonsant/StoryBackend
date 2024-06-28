namespace StoryBackend.Models.DTOs;

    public class AcceptInviteDto
    {
        public Guid? InviteeId { get; set; }
    }
    public class CreateInviteeDto
    {

    }

    public record GetInviteeDto { 
        public Guid? InviteeId { get; set; }
        public string? StoryName { get; set; }
        public Guid? StoryId { get; set; }
        public string? CreatorUsername { get; set; }
        public DateTimeOffset? InvitedDate { get; set; }

        public static GetInviteeDto Instance(Guid? inviteeId, string? storyName, Guid? storyId, string? creatorUsername, DateTimeOffset? invitedDate)
        {
            return new()
            {
                InviteeId = inviteeId,
                StoryName = storyName,
                StoryId = storyId,
                CreatorUsername = creatorUsername,
                InvitedDate = invitedDate
            };
        }
    }
