using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.EntityCommands;
using ECS;
using ECS.Components;
using MediatR;

namespace MonoGameNetworking.ECS.System;

public class NetworkedTransformSystem : ISystem
{
    private readonly World world;
    private readonly InputSystem inputSystem;
    private readonly ClientNetworker clientNetworker;
    
    public NetworkedTransformSystem(World world, InputSystem inputSystem, ClientNetworker clientNetworker)
    {
        this.world = world;
        this.inputSystem = inputSystem;
        this.clientNetworker = clientNetworker;
        
        inputSystem.CommandCreated += Send;
    }
    
    public void Process()
    {
        inputSystem.Process();
    }
    
    private async void Send(object sender, MovementCommand request)
    {
        await clientNetworker.SendCommand(request);
    }
}