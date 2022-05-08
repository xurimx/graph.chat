using graph.chat.server.abstractions;
using graph.chat.server.Data;
using graph.chat.server.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace graph.chat.server.Services;

public class TopicService : ITopicService, IAsyncDisposable
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;

    public TopicService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = dbContextFactory.CreateDbContext();
    }
    
    public Task<List<Topic>> GetTopicsAsync(TopicType topicType)
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
        
        var userId = Guid.Parse(claim.Value);

        _context.Topics
            .Include(x => x.Participants)
            .Where(x => x.TopicType == topicType && x.Participants.Any(y => y.UserId == userId))
            .ToListAsync();

        throw new NotImplementedException();
    }

    public Task<Topic> CreateTopic(TopicType topicType, List<Guid> participants)
    {
        throw new NotImplementedException();
    }

    public Task<Topic> JoinTopic(Guid topicId)
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}