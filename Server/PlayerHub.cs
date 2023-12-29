using Microsoft.AspNetCore.SignalR;

namespace Server;

public class PlayerHub : Hub
{
    /* SERVER SIDE STRUCTURE:
     * All incoming requests are handled using commands via a mediator, 
     * We will have per-command a handler which will have unique logic and knows what to do with the command
     * we will have a static world (singleton potentiolly) storage like in the client-version
     * each handler will have a internal function where it can change data in the storage
     * when the change has been made it will be send up the chain back to the hub
     * the hub after being able to process the information will broadcast the information to all the clients
     * client can recieve requests and will be able to process them on the client side with its own logic.
     *
     * task-based or just using functions will be determined later
     */
}