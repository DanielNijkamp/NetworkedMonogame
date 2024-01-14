using Commands;
using ECS;
using MediatR;

namespace Handlers.Client;

public class CopyGameStateCommandHandler : IRequestHandler<CopyGameStateCommand>
{
    private readonly World World;
    
    public CopyGameStateCommandHandler(World world)
    {
        World = world;
    }
    
    public async Task Handle(CopyGameStateCommand request, CancellationToken cancellationToken)
    {
        World.Replicate(request.Storage);
        Console.WriteLine("Copied gamestate to client");
    }
}