using graph.chat.server.entities;

namespace graph.chat.server.Subscriptions;

public class Subscription
{
    [Subscribe]
    public Message NewMessage([Topic] Guid topicId, [EventMessage] Message message) => message;
}