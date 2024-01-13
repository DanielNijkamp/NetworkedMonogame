using Commands.EntityCommands;
using ECS;
using ECS.Components;
using MediatR;
using Microsoft.Xna.Framework;

namespace Handlers;

public class TransformHandler : IRequestHandler<MovementCommand>
{
    private readonly World world;

    public TransformHandler(World world)
    {
        this.world = world;
    }
    
    public async Task Handle(MovementCommand request, CancellationToken cancellationToken)
    {
        var entity = world.GetEntityById(request.EntityID);
        var component = world.GetComponentOfType<TransformComponent>(entity.Value);
        
        Vector2.Add(component.Position, request.MovementVector);
        var movementVector = Vector2.Normalize(request.MovementVector);
        component.Position += movementVector * component.Velocity;
        Console.WriteLine(component.Position);
    }
}