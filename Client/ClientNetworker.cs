using System.Threading.Tasks;
using Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;

namespace MonoGameNetworking;

public class ClientNetworker
{
    private readonly IMediator mediator;
    private readonly HubConnection hubConnection;


    public ClientNetworker(IMediator mediator, string connectionString)
    {
        this.mediator = mediator;
        hubConnection = new HubConnectionBuilder()
            .WithUrl(connectionString)
            .Build();
        
        hubConnection.StartAsync().Wait();
        
        hubConnection.On<ICommand>("ReceiveCommand", ReceiveCommand);
    }
    public async Task SendCommand(ICommand command)
    {
        await hubConnection.SendAsync("SendCommand", command);
    }

    private async Task ReceiveCommand(ICommand command)
    {
        await mediator.Send(command);
    }
}