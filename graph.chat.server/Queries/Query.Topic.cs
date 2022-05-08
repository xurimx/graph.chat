using graph.chat.server.abstractions;
using graph.chat.server.entities;
using HotChocolate.AspNetCore.Authorization;

namespace graph.chat.server.Queries;

public partial class Query
{
    [Authorize(Roles = new []{"User"})]
    public Task<List<Topic>> GetTopicsAsync([Service]ITopicService topicService, TopicType topicType) => 
        topicService.GetTopicsAsync(topicType);
}