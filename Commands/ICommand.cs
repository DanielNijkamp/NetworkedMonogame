using MediatR;

namespace Commands;

public interface ICommand : IRequest
{
    public Guid EntityID { get; set; }
}