using Commands;
using Commands.EntityCommands;
using ECS;
using ECS.Components;
using MediatR;
using Microsoft.Xna.Framework;

namespace Handlers.Shared;

public class TransformHandler : IRequestHandler<MovementCommand>
{
    private readonly World world;

    public TransformHandler(World world)
    {
        this.world = world;
    }
    
    public async Task Handle(MovementCommand request, CancellationToken cancellationToken)
    {
        var components = world.GetComponentsFromEntity(request.EntityID);
        var component = world.GetComponentOfType<TransformComponent>(components);
        
        component.Position += Vector2.Normalize(request.MovementVector) * component.Velocity;
    }
}