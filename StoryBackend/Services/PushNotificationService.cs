﻿using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StoryBackend.Abstract;
using StoryBackend.Configurations;
using StoryBackend.Database;
using StoryBackend.Models;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Services;

public class PushNotificationService : IPushNotificationService
{
    private readonly FirebaseConfig _firebaseConfig;
    private readonly FirebaseApp _firebaseApp;
    private readonly IServiceProvider _serviceProvider;

    public PushNotificationService(IOptionsMonitor<FirebaseConfig> firebaseOptionsMonitor, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _firebaseConfig = firebaseOptionsMonitor.CurrentValue;
        byte[] binaryData = Convert.FromBase64String(_firebaseConfig.SA);
        string decodedString = System.Text.Encoding.UTF8.GetString(binaryData);
        AppOptions options = new AppOptions()
        {
            Credential = GoogleCredential.FromJson(decodedString),
            ProjectId = "storyfrontend-pwa"
        };
        _firebaseApp = FirebaseApp.Create(options);
 
    }

    public async Task<AddUserPushNotificationTokenDto?> AddUserPushNotificationToken(AddUserPushNotificationTokenDto addUserPushNotificationTokenDto, ClaimsPrincipal claimsPrincipal)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            StoryDbContext storyDbContext = scope.ServiceProvider.GetRequiredService<StoryDbContext>();
            IAuthManagementService authManagementService = scope.ServiceProvider.GetRequiredService<IAuthManagementService>();

            Guid? id = await authManagementService.GetUserId(claimsPrincipal);
            if (id is null) return null;

            UserPushNotificationToken? existingToken = await storyDbContext.UserPushNotificationTokens.
                FirstOrDefaultAsync(pu => 
                pu.Token.Equals(addUserPushNotificationTokenDto.Token) &&
                pu.UserId.Equals(id.Value));

            if (existingToken is not null)
            {
                existingToken.Timestamp = DateTimeOffset.UtcNow;
                await storyDbContext.SaveChangesAsync();
                return existingToken.Adapt<AddUserPushNotificationTokenDto>();
            }

            UserPushNotificationToken newUserToken = UserPushNotificationToken.Instance(id.Value, addUserPushNotificationTokenDto.Token);
            await storyDbContext.UserPushNotificationTokens.AddAsync(newUserToken);
            int changes = await storyDbContext.SaveChangesAsync();

            return changes > 0 ? newUserToken.Adapt<AddUserPushNotificationTokenDto>() : null;
        }
    }

    public async Task<ToggleUserPushNotificationTokenDto?> ToggleUserPushNotificationToken(ToggleUserPushNotificationTokenDto toggleUserPushNotificationTokenDto, ClaimsPrincipal claimsPrincipal)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            StoryDbContext storyDbContext = scope.ServiceProvider.GetRequiredService<StoryDbContext>();
            IAuthManagementService authManagementService = scope.ServiceProvider.GetRequiredService<IAuthManagementService>();

            Guid? id = await authManagementService.GetUserId(claimsPrincipal);
            if (id is null) return null;

            UserPushNotificationToken? existingToken = await storyDbContext.UserPushNotificationTokens.
                FirstOrDefaultAsync(pu =>
                pu.Token.Equals(toggleUserPushNotificationTokenDto.Token) &&
                pu.UserId.Equals(id.Value));

            if (existingToken is null) return null;

            existingToken.Enabled = toggleUserPushNotificationTokenDto.Enabled;
            existingToken.Timestamp = DateTimeOffset.UtcNow;
            int changes = await storyDbContext.SaveChangesAsync();

            return changes > 0 ? existingToken.Adapt<ToggleUserPushNotificationTokenDto>() : toggleUserPushNotificationTokenDto;
        }
    }

    public async Task<GetUserPushNotificationTokenDto?> GetUserPushNotificationToken(string token, ClaimsPrincipal claimsPrincipal)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            StoryDbContext storyDbContext = scope.ServiceProvider.GetRequiredService<StoryDbContext>();
            IAuthManagementService authManagementService = scope.ServiceProvider.GetRequiredService<IAuthManagementService>();

            Guid? id = await authManagementService.GetUserId(claimsPrincipal);
            if (id is null) return null;

            UserPushNotificationToken? existingToken = await storyDbContext.UserPushNotificationTokens.
                FirstOrDefaultAsync(pu =>
                pu.Token.Equals(token) &&
                pu.UserId.Equals(id.Value));

            if (existingToken is null) return null;

            existingToken.Timestamp = DateTimeOffset.UtcNow;
            await storyDbContext.SaveChangesAsync();

            return existingToken.Adapt<GetUserPushNotificationTokenDto>();
        }
    }

    public async Task<bool> SendNotification(PushNotification pushNotification)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            StoryDbContext storyDbContext = scope.ServiceProvider.GetRequiredService<StoryDbContext>();

            FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(_firebaseApp);

            IEnumerable<UserPushNotificationToken> tokens = await GetPushTokensByUserId(pushNotification.UserId, storyDbContext);

            await Parallel.ForEachAsync(tokens, async (token, c) => {
                await Send(pushNotification, token, messaging, storyDbContext);
            });
            return true;
        }
    }

    public async Task<DeleteUserPushNotificationTokenDto?> DeleteUserPushNotificationToken(string token, ClaimsPrincipal claimsPrincipal)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            StoryDbContext storyDbContext = scope.ServiceProvider.GetRequiredService<StoryDbContext>();
            IAuthManagementService authManagementService = scope.ServiceProvider.GetRequiredService<IAuthManagementService>();

            Guid? id = await authManagementService.GetUserId(claimsPrincipal);
            if (id is null) return null;

            UserPushNotificationToken? existingToken = await storyDbContext.UserPushNotificationTokens.
                FirstOrDefaultAsync(pu =>
                pu.Token.Equals(token) &&
                pu.UserId.Equals(id.Value));

            if (existingToken is null) return null;

            storyDbContext.UserPushNotificationTokens.Remove(existingToken);
            int changes = await storyDbContext.SaveChangesAsync();

            return changes > 0 ? DeleteUserPushNotificationTokenDto.Instance(token) : null;
        }
    }

    private async Task<IEnumerable<UserPushNotificationToken>> GetPushTokensByUserId(Guid userId, StoryDbContext storyDbContext)
    {
        return await storyDbContext.UserPushNotificationTokens.Where(up => up.UserId.Equals(userId)).ToListAsync();
    }

    private async Task<bool> Send(PushNotification pushNotification, UserPushNotificationToken token, FirebaseMessaging messaging, StoryDbContext storyDbContext)
    {
        if (token.Enabled is false) return false;
        var msg = new Message()
        {
            Notification = new Notification()
            {
                Title = pushNotification.Title,
                Body = pushNotification.Message
            },
            Token = token.Token
        };
        string? response = null;
        try
        {
            response = await messaging.SendAsync(msg);
        }catch (FirebaseMessagingException ex)
        {
            if (ex.ErrorCode == ErrorCode.NotFound)
            {
                await DeleteToken(token.Token, storyDbContext);
            }
        }
        return response is not null;
    }

    private async Task DeleteToken(string token, StoryDbContext storyDbContext)
    {
        UserPushNotificationToken? userPushNotificationToken = await storyDbContext.UserPushNotificationTokens.FirstOrDefaultAsync(up => up.Token.Equals(token));
        if (userPushNotificationToken is null) return;

        storyDbContext.UserPushNotificationTokens.Remove(userPushNotificationToken);
        await storyDbContext.SaveChangesAsync();
    }
}
