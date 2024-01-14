using Commands.EntityCommands;
using ECS;
using MediatR;

namespace Handlers;

public class CreateEntityHandler : IRequestHandler<CreateEntityCommand>
{
    private readonly World world;
    
    public CreateEntityHandler(World world)
    {
        this.world = world;
    }
    public async Task Handle(CreateEntityCommand request, CancellationToken cancellationToken)
    {
        var result = world.CreateEntity(request.EntityID, request.Components);
        Console.WriteLine($"Entity created: [{request.EntityID}]");
        foreach (var component in request.Components)
        {
            Console.WriteLine($"Entity: [{request.EntityID}] has component: [{component}]");
        }
        
    }
}