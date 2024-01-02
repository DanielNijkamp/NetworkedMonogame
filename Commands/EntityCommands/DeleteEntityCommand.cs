namespace Commands.EntityCommands;

public class DeleteEntityCommand : ICommand
{
    public Guid EntityID { get; set; }
}