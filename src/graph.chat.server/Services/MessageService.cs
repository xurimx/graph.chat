using System.Security.Claims;
using graph.chat.server.abstractions;
using graph.chat.server.Data;
using graph.chat.server.entities;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace graph.chat.server.Services;

public class MessageService : IMessageService
{
    private readonly ITopicEventSender _topicEventSender;
    private readonly ApplicationDbContext _context;
    private readonly Guid _currentUserId;
    
    public MessageService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IHttpContextAccessor httpContextAccessor, ITopicEventSender topicEventSender)
    {
        _topicEventSender = topicEventSender;
        var claim = httpContextAccessor.HttpContext?.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
        
        if (claim?.Value is null)
            throw new Exception("Id not found");
        _currentUserId = Guid.Parse(claim.Value);
        
        _context = dbContextFactory.CreateDbContext();
    }
    
    public async Task<List<Message>> GetMessagesAsync(Guid topicId)
    {
        var isParticipantInTopic = await _context.TopicMembers.AnyAsync(x => x.TopicId == topicId && x.UserId == _currentUserId);
        if (isParticipantInTopic)
            return await _context.Messages.Where(x => x.TopicId == topicId)
                .OrderByDescending(x=> x.Timestamp)
                .ToListAsync();

        return new List<Message>();
    }

    public async Task<Message> SendMessage(Guid topicId, string content)
    {
        var isParticipantInTopic = await _context.TopicMembers.AnyAsync(x => x.TopicId == topicId && x.UserId == _currentUserId);
        if (isParticipantInTopic is false)
            throw new Exception("Current user is not participant in this topic or topic does not exist");
        
        var message = new Message
        {
            Content = content,
            Timestamp = DateTime.UtcNow,
            SenderId = _currentUserId,
            TopicId = topicId
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        await _topicEventSender.SendAsync(topicId, message);

        return message;
    }
}