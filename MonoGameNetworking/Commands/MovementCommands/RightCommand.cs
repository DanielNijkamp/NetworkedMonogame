using Microsoft.Xna.Framework;

namespace MonoGameNetworking.Commands.MovementCommands;

public class RightCommand : ICommand
{
    public RightCommand(uint playerId)
    {
        this.PlayerID = playerId;
    } 
    public uint PlayerID { get; set; }
    public Vector2 MovementVector { get; } = new Vector2(1, 0);
}