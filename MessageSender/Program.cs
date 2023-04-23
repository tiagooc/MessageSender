using Hangfire;
using Hangfire.MemoryStorage;
using MessageSender;
using MessageSender.Repository;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var multiplexer = ConnectionMultiplexer.Connect("localhost");
services.AddSingleton<IConnectionMultiplexer>(multiplexer);
services.AddSingleton<IMessageSender, MessageSender.MessageSender>();
services.AddSingleton<IRepository, RedisRepository>();
services.AddHangfire(configuration => 
    configuration.UseMemoryStorage());

services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHangfireDashboard();
app.Run();