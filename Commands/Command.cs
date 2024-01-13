using Commands.EntityCommands;
using MediatR;

namespace Commands;

[MessagePack.Union(0, typeof(MovementCommand))]
[MessagePack.Union(1, typeof(CreateEntityCommand))]
[MessagePack.Union(2, typeof(DeleteEntityCommand))]
public class Command : IRequest
{
    public Guid EntityID { get; set; }
}