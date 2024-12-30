using Microsoft.AspNetCore.Authorization;
using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.CommandsAndQueries;

public class PushNotificationCommandsAndQueries
{
    public static async Task<AddUserPushNotificationTokenDto?> HandleAddUserPushNotificationToken(IPushNotificationService pushNotificationService, AddUserPushNotificationTokenDto addUserPushNotificationTokenDto, ClaimsPrincipal claimsPrincipal) => await pushNotificationService.AddUserPushNotificationToken(addUserPushNotificationTokenDto, claimsPrincipal);
    public static async Task<ToggleUserPushNotificationTokenDto?> HandleToggleUserPushNotificationToken(IPushNotificationService pushNotificationService, ToggleUserPushNotificationTokenDto toggleUserPushNotificationTokenDto, ClaimsPrincipal claimsPrincipal) => await pushNotificationService.ToggleUserPushNotificationToken(toggleUserPushNotificationTokenDto, claimsPrincipal);
    public static async Task<GetUserPushNotificationTokenDto?> HandleGetUserPushNotificationToken(IPushNotificationService pushNotificationService, string token, ClaimsPrincipal claimsPrincipal) => await pushNotificationService.GetUserPushNotificationToken(token, claimsPrincipal);
    public static async Task<DeleteUserPushNotificationTokenDto?> HandleDeleteUserPushNotificationToken(IPushNotificationService pushNotificationService, string token, ClaimsPrincipal claimsPrincipal) => await pushNotificationService.DeleteUserPushNotificationToken(token, claimsPrincipal);
}
