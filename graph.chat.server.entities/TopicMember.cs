namespace graph.chat.server.entities;

public class TopicMember
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Guid UserId { get; set; }
}