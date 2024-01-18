using Commands.EntityCommands;
using ECS;
using MediatR;

namespace Handlers.Shared;

public class CreateEntityHandler : IRequestHandler<CreateEntityCommand>
{
    private readonly World world;
    
    public CreateEntityHandler(World world)
    {
        this.world = world;
    }
    public async Task Handle(CreateEntityCommand request, CancellationToken cancellationToken)
    {
        world.CreateEntity(request.EntityID, request.Components);
        Console.WriteLine($"Entity created: [{request.EntityID}]");
    }
}