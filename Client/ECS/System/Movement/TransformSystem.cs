using System;
using System.Linq;
using Commands.MovementCommands;
using ECS;
using Microsoft.Xna.Framework;
using MonoGameNetworking.ECS.Components;

namespace MonoGameNetworking.ECS.System.Movement;

public class TransformSystem : ISystem
{
    private readonly World world;
    private readonly InputSystem inputSystem;

    private TransformComponent component;

    public TransformSystem(World world, InputSystem inputSystem)
    {
        this.world = world;
        this.inputSystem = inputSystem;
        
    }

    public void Initialize(Guid EnityTargetID)
    {
        component = world.entities[EnityTargetID].OfType<TransformComponent>().FirstOrDefault()!;
        inputSystem.CommandCreated += SetPosition!;
    }
    
    public void Process()
    {
        inputSystem.Process();
    }

    private void SetPosition(object sender, IMovementCommand command)
    {
        Vector2.Add(component.Position, command.MovementVector);
        var movementVector = Vector2.Normalize(command.MovementVector);
        component.Position += movementVector * component.Velocity;
        Console.WriteLine(component.Position);
    }
}