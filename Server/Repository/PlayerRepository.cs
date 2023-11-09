using Microsoft.Xna.Framework;

namespace Server.Repository;

public class PlayerRepository
{
    private Dictionary<uint, Vector2> playerPositions;
    
    public void SetPlayerPosition(uint playerID,Vector2 pos)
    {
        playerPositions[playerID] = pos;
    }
    //all async tasks
    //keeps kvp of players: [playerID, position]
    //function to modify positional data for specific player
}