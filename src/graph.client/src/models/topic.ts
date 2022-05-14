export class Topic{
    public Text: string;
    public Icon: string;
    public TopicId: string;

    constructor(text: string, icon: string, topicId: string) {
        this.Text = text
        this.Icon = icon
        this.TopicId = topicId
    }
}