using Microsoft.AspNetCore.Identity;
using StoryBackend.Models.DTOs;
using System.Security.Claims;

namespace StoryBackend.Abstract;

public interface IAuthManagementService
{
    public Task<UserRegistrationResponseDto> Register(UserRegistrationRequestDto request);
    public Task<UserLoginResponseDto> Login(UserLoginRequestDto request);
    public Task<ResetPasswordEmailDto?> ResetPasswordEmail(ResetPasswordEmailDto resetPasswordEmailDto);
    public Task<ResetPasswordRequestDto?> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto);
    public Task<IdentityUser?> GetUser(ClaimsPrincipal claimsPrincipal);
    public Task<Guid?> GetUserId(ClaimsPrincipal claimsPrincipal);
    public Task<CreateRoleResponseDto> CreateRole(CreateRoleRequestDto request);
    public Task<IEnumerable<string>> GetRoles();
    public Task<UserLoginResponseDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);


}
