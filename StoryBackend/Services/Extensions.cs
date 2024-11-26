using StoryBackend.Abstract;

namespace StoryBackend.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddStoryServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IStoryService, StoryService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IInviteeService, InviteeService>();
            serviceCollection.AddTransient<IParticipantService, ParticipantService>();
            serviceCollection.AddTransient<ILobbyMessageService, LobbyMessageService>();
            serviceCollection.AddTransient<IAuthManagementService, AuthManagementService>();
            serviceCollection.AddTransient<IEmailWhitelistService, EmailWhitelistService>();
            serviceCollection.AddTransient<ICommonService, CommonService>();

            return serviceCollection;
        }
    }
}
