using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models;

namespace StoryBackend.Services
{
    public class CommonService(StoryDbContext storyDbContext): ICommonService
    {
        public async Task<IEnumerable<string>> GetUsernamesList(IEnumerable<Guid> userIds)
        {
            List<string> usernames = new List<string>();
            foreach (Guid id in userIds)
            {
                string? username = await GetUsernameById(id);
                if (username is not null) usernames.Add(username);
            }
            return usernames;
        }

        public async Task<string?> GetUsernameById(Guid userId)
        {
            User? user = await storyDbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
            return user is null ? null : user.Username;
        }
    }
}
