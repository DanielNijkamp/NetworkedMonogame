using Server.Repository;

namespace Server.Controllers;

public class PositionalController
{
    private readonly PlayerRepository repo;
    //all async tasks
    //recieve
    //broadcast

    public void UpdatePosition()
    {
        //update position for player
        //broadcast to other clients
        //publish to rabbitMQ
    }

    private void BroadCast()
    {
        
    }
}