﻿using StoryBackend.Abstract;

namespace StoryBackend.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddStoryServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IStoryService, StoryService>();

            return serviceCollection;
        }
    }
}