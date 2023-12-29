using System;
using ECS;

namespace MonoGameNetworking.ECS.System.Movement;

public class NetworkedTransformSystem : ISystem
{
    private readonly BaseWorld _baseWorld;
    private readonly InputSystem inputSystem;
    
    public NetworkedTransformSystem(BaseWorld baseWorld, InputSystem inputSystem)
    {
        this._baseWorld = baseWorld;
        this.inputSystem = inputSystem;
    }
    
    
    public void Process()
    {
        throw new NotImplementedException();
    }
    
    
    //async function able to Send a command created by input. 
    //async function able to Recieve a command created by Server
}