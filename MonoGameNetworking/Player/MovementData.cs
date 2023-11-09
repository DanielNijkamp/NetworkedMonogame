using Microsoft.Xna.Framework;

namespace MonoGameNetworking.Player;

public class MovementData
{
    public MovementData(Vector2 startingPos)
    {
        Position = startingPos;
    }
    
    public Vector2 Position { get; set; }
}