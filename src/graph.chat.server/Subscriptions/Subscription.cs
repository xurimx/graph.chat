using graph.chat.server.entities;
using HotChocolate.AspNetCore.Authorization;

namespace graph.chat.server.Subscriptions;

public class Subscription
{
    [Subscribe]
    //[Authorize]
    public Message NewMessage([Topic] Guid topicId, [EventMessage] Message message) => message;
}