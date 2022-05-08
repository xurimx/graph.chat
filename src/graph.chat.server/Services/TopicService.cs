using System.Security.Claims;
using graph.chat.server.abstractions;
using graph.chat.server.Data;
using graph.chat.server.entities;
using Microsoft.EntityFrameworkCore;

namespace graph.chat.server.Services;

public class TopicService : ITopicService, IAsyncDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Guid _currentUserId;

    public TopicService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IHttpContextAccessor httpContextAccessor)
    {
        var claim = httpContextAccessor.HttpContext?.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
        
        if (claim?.Value is null)
            throw new Exception("Id not found");
        _currentUserId = Guid.Parse(claim.Value);
        
        _context = dbContextFactory.CreateDbContext();
    }
    
    public Task<List<Topic>> GetTopicsAsync(TopicType topicType)
    {
        return _context.Topics
            .Include(x => x.Participants)
            .Where(x => x.TopicType == topicType && x.Participants.Any(y => y.UserId == _currentUserId))
            .ToListAsync();
    }

    public async Task<Topic> CreateTopicAsync(TopicType topicType, List<Guid> participants)
    {
        var topic = new Topic
        {
            TopicOwner = _currentUserId,
            TopicType = topicType,
        };
        
        _context.Topics.Add(topic);

        foreach (var participant in participants)
        {
            var participantExists = _context.Users.Any(x => x.Id == participant.ToString());
            if (participantExists is false)
                continue;
            
            var topicMember = new TopicMember
            {
                TopicId = topic.Id,
                UserId = participant
            };
            
            topic.Participants.Add(topicMember);
        }
        
        await _context.SaveChangesAsync();
        
        return topic;
    }

    public async Task<Topic> JoinTopicAsync(Guid topicId)
    {
        var topic = await _context.Topics
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(x => x.Id == topicId);
        
        if (topic is null)
            throw new Exception("Topic does not exist");

        if (topic.Participants.Any(x => x.UserId == _currentUserId))
            return topic;
        
        var topicMember = new TopicMember
        {
            TopicId = topicId,
            UserId = _currentUserId
        };

        topic.Participants.Add(topicMember);

        await _context.SaveChangesAsync();

        return topic;
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}