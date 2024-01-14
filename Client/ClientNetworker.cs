using System;
using System.Threading.Tasks;
using Commands;
using Commands.EntityCommands;
using ECS;
using ECS.Components;
using MediatR;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameNetworking;

public class ClientNetworker
{
    private readonly IMediator mediator;
    private readonly HubConnection hubConnection;

    private readonly Guid EntityID;

    public ClientNetworker(IMediator mediator, string connectionString, Guid PlayerID)
    {
        EntityID = PlayerID;
        this.mediator = mediator;
        hubConnection = new HubConnectionBuilder()
            .WithUrl(connectionString)
            .AddMessagePackProtocol()
            .Build();
        
        hubConnection.On<CommandWrapper>("ReceiveCommandWrapper", ReceiveCommandWrapper);
        hubConnection.On("RequestInit", InitClient);
        hubConnection.On("CreatePlayer", CreatePlayer);
    }
    private async Task InitClient()
    {
        await hubConnection.InvokeAsync("InitClient", EntityID);
        
    }
    private async Task CreatePlayer()
    {
        Console.WriteLine($"Create player command sent: [{EntityID}]");
        await SendCommand(new CreateEntityCommand 
        { 
            EntityID = EntityID, 
            Components = new Component[] 
            { 
                new TransformComponent{Velocity = 5f},
                new RenderComponent{TextureName = "ball"}
            }
        });
    }
    
    public async Task StartConnection()
    {
        await hubConnection.StartAsync();
    }
    public async Task DisconnectAsync()
    {
        await hubConnection.StopAsync();
    }
    public async Task SendCommand<T>(T command) where T : Command
    {
        Console.WriteLine($"Sending [{command}]");
        
        var wrapper = new CommandWrapper
        {
            CommandType = typeof(T).AssemblyQualifiedName,
            Command = command
        };
        await hubConnection.SendAsync("ReceiveCommandWrapper", wrapper);
    }
    
    private async Task ReceiveCommandWrapper(CommandWrapper wrapper)
    {
        Console.WriteLine($"Received [{wrapper.Command}]");
        var type = Type.GetType(wrapper.CommandType);
        var command = MessagePackSerializer.Deserialize(type, MessagePackSerializer.Serialize(wrapper.Command)) as IRequest;
        await mediator.Send(command);
    }
}