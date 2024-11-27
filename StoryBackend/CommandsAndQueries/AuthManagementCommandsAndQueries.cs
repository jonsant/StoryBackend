using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;
using StoryBackend.Services;
using System.Security.Claims;

namespace StoryBackend.CommandsAndQueries;

public class AuthManagementCommandsAndQueries
{
    public static async Task<UserRegistrationResponseDto> HandleRegister(IAuthManagementService authManagementService, UserRegistrationRequestDto userRegistrationRequestDto) => await authManagementService.Register(userRegistrationRequestDto);
    public static async Task<UserLoginResponseDto> HandleLogin(IAuthManagementService authManagementService, UserLoginRequestDto userLoginRequestDto) => await authManagementService.Login(userLoginRequestDto);
    public static async Task<ResetPasswordEmailDto?> HandleResetPasswordEmail(IAuthManagementService authManagementService, ResetPasswordEmailDto resetPasswordEmailDto) => await authManagementService.ResetPasswordEmail(resetPasswordEmailDto);
    public static async Task<ResetPasswordRequestDto?> HandleResetPassword(IAuthManagementService authManagementService, ResetPasswordRequestDto resetPasswordRequestDto) => await authManagementService.ResetPassword(resetPasswordRequestDto);
    public static async Task<CreateRoleResponseDto> HandleCreateRole(IAuthManagementService authManagementService, CreateRoleRequestDto createRoleRequestDto) => await authManagementService.CreateRole(createRoleRequestDto);
    public static async Task<IEnumerable<string>> HandleGetRoles(IAuthManagementService authManagementService) => await authManagementService.GetRoles();
    public static async Task<UserLoginResponseDto> HandleGetCurrentUser(IAuthManagementService authManagementService, ClaimsPrincipal claimsPrincipal) => await authManagementService.GetCurrentUser(claimsPrincipal);
}
