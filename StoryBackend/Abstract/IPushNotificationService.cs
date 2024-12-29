using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IPushNotificationService
{
    Task<bool> SendNotification(PushNotification pushNotification);
    Task<AddUserPushNotificationTokenDto?> AddUserPushNotificationToken(AddUserPushNotificationTokenDto addUserPushNotificationTokenDto, ClaimsPrincipal claimsPrincipal);
    Task<DeleteUserPushNotificationTokenDto?> DeleteUserPushNotificationToken(string token, ClaimsPrincipal claimsPrincipal);
}
