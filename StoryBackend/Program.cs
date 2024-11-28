using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using StoryBackend.Configurations;
using StoryBackend.Database;
using StoryBackend.Endpoints;
using StoryBackend.Services;
using StoryBackend.SignalR;
using System.Text;

var MyAllowOrigins = "_myAllowOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.Configure<AutoAdmins>(builder.Configuration.GetSection("AutoAdmins"));
builder.Services.Configure<SendingEmail>(builder.Configuration.GetSection("SendingEmail"));
builder.Services.Configure<CommonConfig>(builder.Configuration.GetSection("CommonConfig"));

//builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
//builder.Services.AddAuthorizationBuilder().AddPolicy("play_story", policy =>
//{
//    //policy.RequireClaim("scope");
//    policy.RequireScope("Story.Play");
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://jonsant.github.io", "https://localhost:4200", "https://groupwriter.app", "http://groupwriter.app");
            policy.WithHeaders("*");
            policy.WithMethods("*");
            policy.AllowCredentials();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "StoryBackend",
        Version = "v1"
    });
});

builder.Services.AddSignalR();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<IdStoryDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = false
    };

    // Sending the access token in the query string is required when using WebSockets or ServerSentEvents
    // due to a limitation in Browser APIs. We restrict it to only calls to the
    // SignalR hub in this code.
    // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
    // for more information about security considerations when using
    // the query string to transmit the access token.
    jwt.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/storyhub") || path.StartsWithSegments("/userhub")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(o => 
    o.AddPolicy("AdminsOnly", b =>
    b.RequireRole("Admin")
));

// signalR custom userId provider
builder.Services.AddSingleton<IUserIdProvider, UserIdBasedUserIdProvider>();

builder.Services.AddStoryDb(builder);
builder.Services.AddStoryServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
}

//app.UseHttpsRedirection();
app.UseCors(MyAllowOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseStoryEndpoints();
app.UseUserEndpoints();
app.UseInviteeEndpoints();
app.UseLobbyMessageEndpoints();
app.UseAuthManagementEndpoints();
app.UseEmailWhitelistEndpoints();
app.MapHub<StoryHub>("/storyhub/{storyid}");
app.MapHub<UserHub>("/userhub");

app.Run();
