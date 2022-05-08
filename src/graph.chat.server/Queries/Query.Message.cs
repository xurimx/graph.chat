using graph.chat.server.abstractions;
using graph.chat.server.entities;
using HotChocolate.AspNetCore.Authorization;

namespace graph.chat.server.Queries;

public partial class Query
{
    [Authorize]
    public Task<List<Message>> GetMessagesAsync([Service] IMessageService messageService, 
        Guid topicId) =>
        messageService.GetMessagesAsync(topicId);

    public Task<Message> SendMessageAsync([Service] IMessageService messageService,
        Guid topicId, string content) =>
        messageService.SendMessage(topicId, content);
}