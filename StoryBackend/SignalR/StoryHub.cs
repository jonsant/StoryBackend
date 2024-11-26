using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StoryBackend.Abstract;

namespace StoryBackend.SignalR
{
    [Authorize]
    public class StoryHub(IAuthManagementService authManagementService,
        IUserService userService,
        IParticipantService participantService
    ) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User is null) Context.Abort();
            Guid? storyId = Guid.Parse(Context.GetHttpContext()!.GetRouteValue("storyid")!.ToString()!);
            var userId = await authManagementService.GetUserId(Context.User);
            bool isParticipant = await participantService.UserIsStoryParticipant(userId.Value, storyId.Value);
            if (!isParticipant) Context.Abort();
            var user = await userService.GetUserById(userId.Value);

            await Groups.AddToGroupAsync(Context.ConnectionId, storyId.ToString()!);
        }
        //public async Task JoinLobby(string user, string message)
        //{
        //    await Clients.All.SendAsync("UserJoinedLobby", user, message);
        //}

        public async Task LeaveLobby(string storyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, storyId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var a = Context.User;
            //return base.OnDisconnectedAsync(exception);
        }
    }
}
