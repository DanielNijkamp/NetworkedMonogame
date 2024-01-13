using System;
using System.Linq;
using Commands.EntityCommands;
using ECS;
using ECS.Components;
using Microsoft.Xna.Framework;

namespace MonoGameNetworking.ECS.System;

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

    public void Initialize(Guid enityTargetId)
    {
        component = world.GetEntityById(enityTargetId).Value.OfType<TransformComponent>().FirstOrDefault()!;
        inputSystem.CommandCreated += SetPosition!;
    }
    
    public void Process()
    {
        inputSystem.Process();
    }

    private void SetPosition(object sender, MovementCommand command)
    {
        Vector2.Add(component.Position, command.MovementVector);
        var movementVector = Vector2.Normalize(command.MovementVector);
        component.Position += movementVector * component.Velocity;
        Console.WriteLine(component.Position);
    }
}