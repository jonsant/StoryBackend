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

            return serviceCollection;
        }
    }
}
