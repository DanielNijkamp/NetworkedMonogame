using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameNetworking.Commands;

namespace MonoGameNetworking.Player;

public class ClientController : IPlayerController
{
    private MovementData movementData;
    private IKeyInputRouter inputRouter;
    
    public ClientController(MovementData movementData, IKeyInputRouter inputRouter)
    {
        this.movementData = movementData;
        this.inputRouter = inputRouter;

        this.inputRouter.CommandCreated += UpdateMovementData;
    }
    
    public void UpdateMovementData(object sender, ICommand command)
    {
       movementData.Position = Vector2.Add(movementData.Position, command.MovementVector);
    }
}