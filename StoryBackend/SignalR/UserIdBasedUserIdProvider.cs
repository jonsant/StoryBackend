using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace StoryBackend.SignalR
{
    public class UserIdBasedUserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("Id")!.Value;
        }
    }
}
