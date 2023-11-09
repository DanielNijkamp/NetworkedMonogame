using System;
using Microsoft.Xna.Framework;

namespace MonoGameNetworking.Commands;

public interface ICommand
{
    public uint PlayerID { get; set; }
    public Vector2 MovementVector { get; }
}