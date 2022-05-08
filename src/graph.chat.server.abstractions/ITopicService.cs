using graph.chat.server.entities;

namespace graph.chat.server.abstractions;

public interface ITopicService
{
    Task<List<Topic>> GetTopicsAsync(TopicType topicType);

    Task<Topic> CreateTopic(TopicType topicType, List<Guid> participants);

    Task<Topic> JoinTopic(Guid topicId);
}