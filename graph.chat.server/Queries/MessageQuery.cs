using graph.chat.server.abstractions;
using graph.chat.server.entities;

namespace graph.chat.server.Queries;

public class MessageQuery
{
    public Task<List<Message>> GetMessages(IMessageService messageService, Guid topicId) =>
        messageService.GetMessagesAsync(topicId);
}