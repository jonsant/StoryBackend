namespace StoryBackend.Models
{
    public class Story
    {
        public Guid StoryId { get; set; }
        public string? StoryName { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Finished { get; set; }
        public string Status { get; set; } = string.Empty; // Created, Active, Finished
        //public List<Participant>? Participants { get; set; } = Enumerable.Empty<Participant>().ToList();
        //public List<Invitee>? Invitees { get; set; } = Enumerable.Empty<Invitee>().ToList();
        //public List<LobbyMessage>? LobbyMessages { get; set; } = Enumerable.Empty<LobbyMessage>().ToList();
        //public List<StoryEntry>? StoryEntries { get; set;} = Enumerable.Empty<StoryEntry>().ToList();
        public int? CurrentPlayerInOrder { get; set; }
        public Guid? CurrentPlayerId {  get; set; }
        public string? FinalStory { get; set; }

        public static Story Instance(string storyName, Guid creatorUserId, string status)
        {
            return new Story()
            {
                StoryName = storyName,
                CreatorUserId = creatorUserId,
                Created = DateTimeOffset.Now,
                Status = status
            };
        }
    }
}
