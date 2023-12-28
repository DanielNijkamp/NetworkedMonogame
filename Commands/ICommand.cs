namespace Commands;

public interface ICommand
{
    public Guid EntityID { get; set; }
}