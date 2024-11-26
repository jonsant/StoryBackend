using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StoryBackend.Abstract;

namespace StoryBackend.SignalR
{
    [Authorize]
    public class UserHub(IAuthManagementService authManagementService,
        IUserService userService
    ) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User is null) Context.Abort();
            var userId = await authManagementService.GetUserId(Context.User);
            if (userId == null) Context.Abort();
            
            //Guid? storyId = Guid.Parse(Context.GetHttpContext()!.GetRouteValue("storyid")!.ToString()!);
            //bool isParticipant = await participantService.UserIsStoryParticipant(userId.Value, storyId.Value);
            //if (!isParticipant) return;
            //var user = await userService.GetUserById(userId.Value);

            //await Groups.AddToGroupAsync(Context.ConnectionId, storyId.ToString()!);
        }

        //public async Task LeaveLobby(string storyId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, storyId);
        //}

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var a = Context.User;
            //return base.OnDisconnectedAsync(exception);
        }
    }
}
