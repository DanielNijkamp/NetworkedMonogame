using Commands;
using ECS;
using MediatR;

namespace Handlers.Client;

public class CopyGameStateCommandHandler : IRequestHandler<CopyGameStateCommand>
{
    private readonly World world;
    
    public CopyGameStateCommandHandler(World world)
    {
        this.world = world;
    }
    
    public async Task Handle(CopyGameStateCommand request, CancellationToken cancellationToken)
    {
        world.Replicate(request.Storage);
        Console.WriteLine("Copied gamestate to client");
    }
}