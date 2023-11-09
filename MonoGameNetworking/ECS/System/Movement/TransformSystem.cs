using System;
using Microsoft.Xna.Framework;
using MonoGameNetworking.ECS.Components;
using MonoGameNetworking.Commands;
using MonoGameNetworking.Commands.MovementCommands;


namespace MonoGameNetworking.ECS.System;

public class TransformSystem : ISystem
{
    private readonly EntityManager entityManager;
    private readonly InputSystem inputSystem;

    private TransformComponent component;

    public TransformSystem(EntityManager entityManager, InputSystem inputSystem)
    {
        this.entityManager = entityManager;
        this.inputSystem = inputSystem;
        
    }

    public void Initialize(Guid EnityTargetID)
    {
        var entity = this.entityManager.GetEntityById(EnityTargetID);
        entity.GetComponent(out component);
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