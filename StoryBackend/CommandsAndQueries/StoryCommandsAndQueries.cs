using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.CommandsAndQueries
{
    public class StoryCommandsAndQueries
    {
        public static async Task<IEnumerable<GetWeatherForecastDto>> HandleGetForecastBackendTest(IStoryService storyService) => await storyService.GetForecastBackendTest();
        public static async Task<IEnumerable<GetStoryDto>> HandleGetStoriesByUserId(IStoryService storyService, ClaimsPrincipal user) => await storyService.GetStoriesByUserId(user);
        public static async Task<IEnumerable<GetStoryDto>> HandleGetParticipantStoriesByUserId(IStoryService storyService, ClaimsPrincipal user) => await storyService.GetParticipantStoriesByUserId(user);
        public static async Task<GetStoryDto> HandleGetStoryById(IStoryService storyService, string storyId, ClaimsPrincipal user) => await storyService.GetStoryById(storyId, user);
        public static async Task<GetWeatherForecastDto> HandleCreateForecastBackendTest(IStoryService storyService, CreateWeatherForecast weatherForecastDto) => await storyService.CreateForecastTest(weatherForecastDto);
        public static async Task<GetStoryDto> HandleCreateStory(IStoryService storyService, CreateStoryDto createStoryDto, ClaimsPrincipal user) => await storyService.CreateStory(createStoryDto, user);
        public static async Task<StartStoryDto> HandleStartStory(IStoryService storyService, StartStoryDto startStoryDto, ClaimsPrincipal user) => await storyService.StartStory(startStoryDto, user);
        public static async Task<CreateEntryDto?> HandleEndStory(IStoryService storyService, CreateEntryDto createEntryDto, ClaimsPrincipal user) => await storyService.EndStory(createEntryDto, user);
        public static async Task<CreateEntryDto?> HandleCreateEntry(IStoryService storyService, CreateEntryDto createEntryDto, ClaimsPrincipal user) => await storyService.CreateEntry(createEntryDto, user);
    }
}
