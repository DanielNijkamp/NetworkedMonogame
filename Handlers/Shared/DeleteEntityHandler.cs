using Commands.EntityCommands;
using ECS;
using MediatR;

namespace Handlers.Shared;

public class DeleteEntityHandler : IRequestHandler<DeleteEntityCommand>
{
    private readonly World world;
    
    public DeleteEntityHandler(World world)
    {
        this.world = world;
    }
    
    public async Task Handle(DeleteEntityCommand request, CancellationToken cancellationToken)
    {
        world.DeleteEntity(request.EntityID);
        Console.WriteLine($"Deleted entity: [{request.EntityID}]");
    }
}