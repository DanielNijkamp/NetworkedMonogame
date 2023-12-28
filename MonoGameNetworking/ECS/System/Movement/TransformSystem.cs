using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGameNetworking.ECS.Components;
using MonoGameNetworking.Commands;
using MonoGameNetworking.Commands.MovementCommands;


namespace MonoGameNetworking.ECS.System;

public class TransformSystem : ISystem
{
    private readonly World _world;
    private readonly InputSystem inputSystem;

    private TransformComponent component;

    public TransformSystem(World world, InputSystem inputSystem)
    {
        this._world = world;
        this.inputSystem = inputSystem;
        
    }

    public void Initialize(Guid EnityTargetID)
    {
        component = this._world.entities[EnityTargetID].OfType<TransformComponent>().FirstOrDefault();
        inputSystem.CommandCreated += SetPosition;
    }
    
    public void Process()
    {
        inputSystem.Process();
    }

    private void SetPosition(object sender, IMovementCommand command)
    {
        var Vector = Vector2.Add(component.Position, command.MovementVector);
        var movementVector = Vector2.Normalize(command.MovementVector);
        component.Position += movementVector * component.Velocity;
        Console.WriteLine(component.Position);
    }
}