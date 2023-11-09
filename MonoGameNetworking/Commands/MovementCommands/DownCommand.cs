using System;
using Microsoft.Xna.Framework;

namespace MonoGameNetworking.Commands.MovementCommands;

public class DownCommand : IMovementCommand
{
    public Guid EntityID { get; set; }
    public Vector2 MovementVector { get;} = new Vector2(0, 1);
}