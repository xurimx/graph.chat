using graph.chat.server.entities;
using Microsoft.AspNetCore.Identity;

namespace graph.chat.server.Data;

public static class DataSeeder
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        using var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();

        var identityUser = new IdentityUser
        {
            UserName = "Urim",
        };
        
        var identityUser2 = new IdentityUser
        {
            UserName = "Batman",
        };
        
        userManager?.CreateAsync(identityUser, "P@ssw0rd").ConfigureAwait(false).GetAwaiter().GetResult();
        userManager?.CreateAsync(identityUser2, "P@ssw0rd").ConfigureAwait(false).GetAwaiter().GetResult();


        var topic = new Topic()
        {
            TopicType = TopicType.Direct,
        };

        context?.Topics.Add(topic);
        
        var participants = new List<TopicMember>()
        {
            new(){UserId = Guid.Parse(identityUser.Id)},
            new(){UserId = Guid.Parse(identityUser2.Id)}
        };
        
        topic.Participants.AddRange(participants);

        context?.SaveChangesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }
}