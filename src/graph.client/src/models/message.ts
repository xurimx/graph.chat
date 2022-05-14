export class Message{
    public Id: string;
    public TopicId: string;
    public SenderId: string;
    public Content: string;
    public Timestamp: string;
    
    constructor(id: string, topicId: string, senderId: string, content: string, timestamp: string) {
        this.Id = id;
        this.TopicId = topicId;
        this.SenderId = senderId;
        this.Content = content;
        this.Timestamp = timestamp
    }
}