using System.Text;
using System.Text.Json;
using Commands;
using Leaderboard.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Leaderboard;

public class CommandConsumer : BackgroundService
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly EventingBasicConsumer consumer;
    private readonly LeaderboardService leaderboardService;

    public CommandConsumer(LeaderboardService leaderboardService)
    {
        this.leaderboardService = leaderboardService;
        
        var factory = new ConnectionFactory() { HostName = "localhost" };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();

        channel.QueueDeclare(queue: "MoveCommands",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        consumer = new EventingBasicConsumer(channel);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var moveCommand = JsonSerializer.Deserialize<MoveCommand>(message);
            
            leaderboardService.UpdateLeaderboard(moveCommand.EntityID);
        };

        channel.BasicConsume(queue: "MoveCommands",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
    
}