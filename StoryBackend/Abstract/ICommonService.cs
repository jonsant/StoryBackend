namespace StoryBackend.Abstract
{
    public interface ICommonService
    {
        Task<IEnumerable<string>> GetUsernamesList(IEnumerable<Guid> userIds);
        Task<IDictionary<Guid, string>> GetUserIdUsernameDict(IEnumerable<Guid> userIds);
        Task<string?> GetUsernameById(Guid userId);
    }
}
