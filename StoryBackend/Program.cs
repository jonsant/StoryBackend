using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using StoryBackend.Configurations;
using StoryBackend.Database;
using StoryBackend.Endpoints;
using StoryBackend.Services;
using System.Text;

var MyAllowOrigins = "_myAllowOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.Configure<AutoAdmins>(builder.Configuration.GetSection("AutoAdmins"));

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
            policy.WithOrigins("http://localhost:4200", "https://jonsant.github.io", "https://localhost:4200");
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
});

builder.Services.AddAuthorization(o => 
    o.AddPolicy("AdminsOnly", b =>
    b.RequireRole("Admin")
));

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

app.Run();
