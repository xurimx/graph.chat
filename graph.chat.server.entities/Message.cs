namespace graph.chat.server.entities;

public class Message
{
    public Guid Id { get; set; }
    public Topic? Topic { get; set; }
    public Guid TopicId { get; set; }
    public Guid SenderId { get; set; }
    public string? Content { get; set; }
}