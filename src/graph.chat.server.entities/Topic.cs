namespace graph.chat.server.entities;

public class Topic
{
    public Guid Id { get; set; }
    public List<TopicMember> Participants { get; set; } = new List<TopicMember>();
    public TopicType TopicType { get; set; }
    public Guid TopicOwner { get; set; }
}