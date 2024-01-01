using ECS;

namespace Commands.EntityCommands;

public class CreateEntityCommand : ICommand
{
    public Guid EntityID { get; set; }

    public IComponent[] Components { get; set; }
}