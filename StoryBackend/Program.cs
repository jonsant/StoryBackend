using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using StoryBackend.Database;
using StoryBackend.Endpoints;
using StoryBackend.Services;

var MyAllowOrigins = "_myAllowOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorizationBuilder().AddPolicy("play_story", policy =>
{
    //policy.RequireClaim("scope");
    policy.RequireScope("Story.Play");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://jonsant.github.io");
            policy.WithHeaders("*");
            policy.WithMethods("*");
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
builder.Services.AddStoryDb(builder);
builder.Services.AddStoryServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowOrigins);
app.UseStoryEndpoints();
app.UseUserEndpoints();
app.UseInviteeEndpoints();
app.UseLobbyMessageEndpoints();

app.Run();
