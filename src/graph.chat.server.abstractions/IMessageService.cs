using graph.chat.server.entities;

namespace graph.chat.server.abstractions;

public interface IMessageService
{
    Task<List<Message>> GetMessagesAsync(Guid topicId);

    Task<Message> SendMessage(Guid topicId, string content);
}