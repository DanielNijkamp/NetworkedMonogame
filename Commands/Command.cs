using Commands.EntityCommands;
using MediatR;

namespace Commands;

[MessagePack.Union(0, typeof(MovementCommand))]
[MessagePack.Union(1, typeof(CreateEntityCommand))]
[MessagePack.Union(2, typeof(DeleteEntityCommand))] 
[MessagePack.Union(3, typeof(CopyGameStateCommand))]
public class Command : IRequest
{
   
}