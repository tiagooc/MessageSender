using Hangfire;
using MessageSender.Models;
using MessageSender.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MessageSender.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IBackgroundJobClient _backgroundJobs;
    private readonly IMessageSender _messageSender;

    public MessagesController(IRepository repository, IBackgroundJobClient backgroundJobs, IMessageSender messageSender)
    {
        _repository = repository;
        _backgroundJobs = backgroundJobs;
        _messageSender = messageSender;
    }

    [HttpPost(Name = "PostMessage")]
    public ActionResult<Message> PostMessage(MessageRequest messageRequest)
    {
        var message = new Message(messageRequest.Body);
        var delay = message.SendTime.DelayUntil();

        _repository.Set(message.Id.ToString(), message);
        _backgroundJobs.Schedule(() =>  _messageSender.SendMessage(message),
            delay);

        return message;
    }

    [HttpPatch("{id:guid}")]
    public ActionResult<Message> PatchMessage(Guid id, MessageRequest messageRequest)
    {
        var message = new Message(id, messageRequest.Body);
        var delay = message.SendTime.DelayUntil();

        _repository.Set(message.Id.ToString(), message);
        _backgroundJobs.Schedule(
            () => _messageSender.SendMessage(message),
            delay);

        return message;
    }
}