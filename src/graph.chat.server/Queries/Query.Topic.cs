using graph.chat.server.abstractions;
using graph.chat.server.entities;
using HotChocolate.AspNetCore.Authorization;

namespace graph.chat.server.Queries;

public partial class Query
{
    [Authorize(Roles = new []{"User"})]
    public Task<List<Topic>> GetTopicsAsync([Service]ITopicService topicService, 
        TopicType topicType) => 
        topicService.GetTopicsAsync(topicType);
    
    [Authorize(Roles = new []{"User"})]
    public Task<Topic> CreateTopicAsync([Service]ITopicService topicService, 
        TopicType topicType, List<Guid> participants) => 
        topicService.CreateTopicAsync(topicType, participants);

    [Authorize(Roles = new []{"User"})]
    public Task<Topic> JoinTopic([Service]ITopicService topicService, 
        Guid topicId) => 
        topicService.JoinTopicAsync(topicId);

}