using graph.chat.server.entities;

namespace graph.chat.server.abstractions;

public interface ITopicService
{
    Task<List<Topic>> GetTopicsAsync(TopicType topicType);

    Task<Topic> CreateTopicAsync(TopicType topicType, List<Guid> participants);

    Task<Topic> JoinTopicAsync(Guid topicId);
}