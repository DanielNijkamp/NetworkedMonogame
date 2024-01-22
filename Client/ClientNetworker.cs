using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Commands;
using Commands.EntityCommands;
using ECS;
using ECS.Components;
using Leaderboard.DTO;
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
        await hubConnection.InvokeAsync("CopyGameState");
    }
    public async Task DisconnectAsync()
    {
        await hubConnection.StopAsync();
    }
    public async Task SendCommand<T>(T command) where T : Command
    {
        var wrapper = new CommandWrapper
        {
            CommandType = typeof(T).AssemblyQualifiedName,
            Command = command
        };
        await hubConnection.SendAsync("ReceiveCommandWrapper", wrapper);
    }
    
    private async Task ReceiveCommandWrapper(CommandWrapper wrapper)
    {
        var type = Type.GetType(wrapper.CommandType);
        var command = MessagePackSerializer.Deserialize(type, MessagePackSerializer.Serialize(wrapper.Command)) as IRequest;
        await mediator.Send(command);
    }
    public async Task QueryLeaderboard()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        using (var client = new HttpClient(handler))
        {
            client.BaseAddress = new Uri("https://localhost:5227");
            try
            {
                var response = await client.GetAsync("/leaderboard");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + stringResult);
                var leaderboardData = JsonSerializer.Deserialize<LeaderboardQueryDto>(stringResult);
                Console.WriteLine($"{leaderboardData.Leaderboard.Keys}, {leaderboardData.Leaderboard.Values}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e + " thrown: " + e.Message);
            }
        }
            
    }
}