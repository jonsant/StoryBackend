﻿using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IParticipantService
{
    public Task<GetParticipantDto> CreateParticipant(CreateParticipantDto createParticipantDto);
    public Task<IEnumerable<GetParticipantDto>> GetParticipants(Guid storyId, ClaimsPrincipal user);
    public Task<bool> UserIsStoryParticipant(Guid userId, Guid storyId);
    public Task<IEnumerable<Guid>> GetStoryParticipantIds(Guid storyId);

}
