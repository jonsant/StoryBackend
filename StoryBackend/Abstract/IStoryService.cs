﻿using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IStoryService
{
    public Task<IEnumerable<GetWeatherForecastDto>> GetForecastBackendTest();
    public Task<IEnumerable<GetStoryDto>> GetStoriesByUserId(ClaimsPrincipal user);
    public Task<IEnumerable<GetStoryDto>> GetParticipantStoriesByUserId(ClaimsPrincipal user);
    public Task<GetStoryDto> GetStoryById(string storyId, ClaimsPrincipal user);
    public Task<GetWeatherForecastDto> CreateForecastTest(CreateWeatherForecast createWeatherForecastDto);
    public Task<GetStoryDto> CreateStory(CreateStoryDto createStoryDto, ClaimsPrincipal user);
    public Task<StartStoryDto> StartStory(StartStoryDto startStoryDto, ClaimsPrincipal user);
    public Task<CreateEntryDto?> CreateEntry(CreateEntryDto createEntryDto, ClaimsPrincipal user);
    public Task<CreateEntryDto?> EndStory(CreateEntryDto createEntryDto, ClaimsPrincipal user);
}
