using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Server.Repository;

public class PlayerRepository
{
    private Dictionary<Guid, Vector2> playerPositions;
    
    public void SetPlayerPosition(Guid EntityID,Vector2 pos)
    {
        playerPositions[EntityID] = pos;
    }

    public Dictionary<Guid, Vector2> GetPositions()
    {
        return playerPositions;
    }
}