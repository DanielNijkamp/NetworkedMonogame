using System.Text;
using System.Text.Json;
using Commands;
using RabbitMQ.Client;

namespace Server;

public class CommandPublisher
{
    private readonly IConnection connection;
    private readonly IModel channel;
    
    public CommandPublisher()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "MoveCommands",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    public void PublishMoveCommand(MoveCommand moveCommand)
    {
        var message = JsonSerializer.Serialize(moveCommand);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: "MoveCommands",
            basicProperties: null,
            body: body);
    }
}