using Newtonsoft.Json;

namespace MessageSender.Models;

public class Message
{
    public Guid Id { get; set; }
    public string Body { get; set; }
    public DateTime SendTime { get; set; }

    private const int DelayInSeconds = 30;

    public Message()
    {
        
    }
    
    [JsonConstructor]
    public Message(Guid id, string body, DateTime sendTime)
    {
        Id = id;
        Body = body;
        SendTime = sendTime;
    }

    public Message(Guid id, string body)
    {
        Id = id;
        Body = body;
        SendTime = DateTime.UtcNow.AddSeconds(DelayInSeconds);
    }

    public Message(string body)
    {
        Id = Guid.NewGuid();
        Body = body;
        SendTime = DateTime.UtcNow.AddSeconds(DelayInSeconds);
    }
}