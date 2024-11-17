using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoryBackend.Abstract;
using StoryBackend.Configurations;
using StoryBackend.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoryBackend.Services;

public class AuthManagementService: IAuthManagementService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtConfig _jwtConfig;
    private readonly AutoAdmins _autoAdmins;
    private readonly IServiceProvider _serviceProvider;
    public AuthManagementService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptionsMonitor<JwtConfig> optionsMonitor,
        IOptionsMonitor<AutoAdmins> autoAdminsMonitor,
        IServiceProvider serviceProvider)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtConfig = optionsMonitor.CurrentValue;
        _autoAdmins = autoAdminsMonitor.CurrentValue;
        _serviceProvider = serviceProvider;
    }

    public async Task<UserRegistrationResponseDto> Register(UserRegistrationRequestDto request)
    {
        IEmailWhitelistService? emailWhitelistService = _serviceProvider.GetRequiredService<IEmailWhitelistService>();
        if (!await emailWhitelistService.EmailExists(request.Email)) return null;
        var emailExist = await _userManager.FindByEmailAsync(request.Email);
        if (emailExist is not null) return new UserRegistrationResponseDto()
        {
            Result = false,
            Errors = new List<string>() { "Email already exists" }
        }; // email already exists

        var newUser = new IdentityUser()
        {
            Email = request.Email,
            UserName = request.Email
        };
        var isCreated = await _userManager.CreateAsync(newUser, request.Password);

        if (!isCreated.Succeeded)
        {
            return new UserRegistrationResponseDto()
            {
                Result = false,
                Errors = isCreated.Errors.Select(x => x.Description).ToList()
            }; // failed creating the user

        }

        // Add roles
        List<string> currentRoles = (await CheckRoles(newUser)).ToList();

        // Create User
        IUserService? userService = _serviceProvider.GetRequiredService<IUserService>();
        GetUserDto? createdUser = await userService.CreateUser(CreateUserDto.Instance(Guid.Parse(newUser.Id)));

        // generate token
        var token = await GenerateJwtToken(newUser);
        var response = new UserRegistrationResponseDto()
        {
            Result = true,
            Token = token,
            Username = createdUser.Username,
            Email = newUser.Email,
            UserId = createdUser.UserId,
            Roles = currentRoles
        };
        return response;
    }

    public async Task<UserLoginResponseDto> Login(UserLoginRequestDto request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is null) return new UserLoginResponseDto()
        {
            Result = false,
            Errors = new List<string>() { "Invalid authentication" }
        };

        var validPassword = await _userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!validPassword) return new UserLoginResponseDto()
        {
            Result = false,
            Errors = new List<string> { "Invalid authentication" }
        };

        // Get/Add roles
        List<string> currentRoles = (await CheckRoles(existingUser)).ToList();

        // Get User
        IUserService? userService = _serviceProvider.GetRequiredService<IUserService>();
        GetUserDto? loggedInUser = await userService.GetUserById(Guid.Parse(existingUser.Id));

        var token = await GenerateJwtToken(existingUser);
        return new UserLoginResponseDto()
        {
            Result = true,
            Token = token,
            Username = loggedInUser.Username,
            Email = existingUser.Email,
            UserId = loggedInUser.UserId,
            Roles = currentRoles
        };
        
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        List<Claim> claims = new List<Claim>()
        {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (string role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }

    public async Task<IdentityUser?> GetUser(ClaimsPrincipal claimsPrincipal)
    {
        var userEmail = _userManager.GetUserId(claimsPrincipal);
        if (userEmail is null) return null;

        var user = await _userManager.FindByEmailAsync(userEmail);
        return user is null ? null : user;
    }

    public async Task<Guid?> GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        IdentityUser? user = await GetUser(claimsPrincipal);
        if (user is null) return null;
        return Guid.Parse(user.Id);
    }

    public async Task<CreateRoleResponseDto> CreateRole(CreateRoleRequestDto request)
    {
        IdentityRole identityRole = new IdentityRole()
        {
            Name = request.Role
        };
        var result = await _roleManager.CreateAsync(identityRole);

        if (!result.Succeeded) return new CreateRoleResponseDto()
        {
            Errors = "Error creating role"
        };

        return new CreateRoleResponseDto()
        {
            Role = identityRole.Name
        };
    }

    public async Task<IEnumerable<string>> GetRoles()
    {
        List<string> roles = Enumerable.Empty<string>().ToList();
        roles = await _roleManager.Roles.Select(r => r.Name ?? "RoleWithoutName").ToListAsync();
        return roles;
    }

    /// <summary>
    /// Adds a user to all existing roles if not already a member.
    /// Only adds user to Admin role if email is found in configuration secrets.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>A list of the users roles</returns>
    private async Task<IEnumerable<string>> CheckRoles(IdentityUser user)
    {
        List<string> currentUserRoles = (await _userManager.GetRolesAsync(user)).Select(r => r.ToLower()).ToList();
        List<string> autoAdminEmails = _autoAdmins.Emails.Split(";").ToList();
        List<string?> roles = await _roleManager.Roles.Where(r => !string.IsNullOrEmpty(r.Name)).Select(r => r.Name).ToListAsync();
        foreach (string? role in roles)
        {
            if (string.IsNullOrEmpty(role)) continue;
            if (currentUserRoles.Contains(role.ToLower())) continue;
            if (role.ToLower().Equals("admin") && !autoAdminEmails.Contains(user.Email!.ToLower())) continue;
            await _userManager.AddToRoleAsync(user, role);
        }

        var updatedUserRoles = await _userManager.GetRolesAsync(user);
        return updatedUserRoles is null ? Enumerable.Empty<string>() : updatedUserRoles.ToList();
    }
}
