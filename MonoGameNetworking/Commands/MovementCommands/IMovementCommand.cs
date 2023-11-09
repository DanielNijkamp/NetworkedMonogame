using Microsoft.Xna.Framework;

namespace MonoGameNetworking.Commands.MovementCommands;

public interface IMovementCommand : ICommand
{
    public Vector2 MovementVector { get; }
}