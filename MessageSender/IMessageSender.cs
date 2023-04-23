using MessageSender.Models;

namespace MessageSender;

public interface IMessageSender
{
    Task SendMessage(Message message);
}