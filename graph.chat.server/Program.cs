using System.Text;
using graph.chat.server.abstractions;
using graph.chat.server.Data;
using graph.chat.server.Queries;
using graph.chat.server.Services;
using graph.chat.server.Subscriptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes("super@secret@secret"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
    {
        options.RequireAuthenticatedSignIn = false;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddSha256DocumentHashProvider();

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(
    options => options.UseInMemoryDatabase("dev"))
    .AddDbContextPool<ApplicationDbContext>( 
    options => options.UseInMemoryDatabase("dev"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITopicService, TopicService>();

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
    .AddSubscriptionType<Subscription>();

builder.Services.AddInMemorySubscriptions();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGraphQL();
});

app.Run();