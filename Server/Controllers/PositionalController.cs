using Server.Repository;

namespace Server.Controllers;

public class PositionalController
{
    private readonly PlayerRepository repo;
    
    public void UpdatePosition()
    {
        //Recieval function
        
        //process request on server via repository
        //broadcast to other clients including original Sender
        //publish to rabbitMQ for leaderboard
    }

    private void BroadCast()
    {
        //Broadcast back to complete round-trip of the Original Sending by calling to ClientTransformSystem
        //Should broacast to all client's NetworkedTransformSystem
    }
}