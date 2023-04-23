using MessageSender.Models;
using MessageSender.Repository;

namespace MessageSender;

public class MessageSender : IMessageSender
{
    private readonly IRepository _repository;

    public MessageSender(IRepository repository)
    {
        _repository = repository;
    }

    public async Task SendMessage(Message message)
    {
        var messageId = message.Id.ToString();
        var messageFromRedis = await _repository.Get<Message>(messageId);

        if (messageFromRedis.SendTime != message.SendTime)
        {
            Console.WriteLine($"Message {messageId} was changed. It will not be sent");
            return;
        }

        Console.WriteLine($"Message {messageId} sent: {message.Body}");
        await _repository.Delete(messageId);
    }
}