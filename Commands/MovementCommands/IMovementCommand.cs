using MediatR;
using Microsoft.Xna.Framework;

namespace Commands.MovementCommands;

public interface IMovementCommand : ICommand
{
    public Vector2 MovementVector { get; }
}