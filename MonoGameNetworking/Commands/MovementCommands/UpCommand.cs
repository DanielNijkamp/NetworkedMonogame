using Microsoft.Xna.Framework;

namespace MonoGameNetworking.Commands.MovementCommands;

public class UpCommand : ICommand
{
    public UpCommand(uint playerId)
    {
        this.PlayerID = playerId;
    } 
    public uint PlayerID { get; set; }
    public Vector2 MovementVector { get; } = new Vector2(0, -1);
}