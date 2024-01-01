using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.MovementCommands;
using ECS;
using MediatR;
using MonoGameNetworking.ECS.Components;

namespace MonoGameNetworking.ECS.System.Movement;

public class NetworkedTransformSystem : ISystem , IRequestHandler<IMovementCommand>
{
    private readonly BaseWorld world;
    private readonly InputSystem inputSystem;
    private readonly ClientNetworker clientNetworker;
    
    public NetworkedTransformSystem(BaseWorld world, InputSystem inputSystem, ClientNetworker clientNetworker)
    {
        this.world = world;
        this.inputSystem = inputSystem;
        this.clientNetworker = clientNetworker;
    }
    
    public void Process()
    {
        inputSystem.Process();
        inputSystem.CommandCreated += Send!;
    }
    
    private async void Send(object sender, IMovementCommand request)
    {
        await clientNetworker.SendCommand(request);
    }
    
    public async Task Handle(IMovementCommand request, CancellationToken cancellationToken)
    {
        var entity = world.GetEntityById(request.EntityID);
        var transformComponent = entity.Value.OfType<TransformComponent>().FirstOrDefault();
        transformComponent.Position = request.MovementVector;
    }
}