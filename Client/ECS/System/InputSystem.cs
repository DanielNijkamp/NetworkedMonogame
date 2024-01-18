using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Commands;
using Commands.EntityCommands;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameNetworking.ECS.System;

public class InputSystem : ISystem
{
    public event EventHandler<MovementCommand>? CommandCreated;
    
    private readonly Dictionary<Keys, MovementCommand> keyCommandMap;

    public InputSystem(Guid entityId)
    {
        keyCommandMap = new Dictionary<Keys, MovementCommand>
        {
            {Keys.W, new MovementCommand{EntityID = entityId, MovementVector = new Vector2(0, -1)}},
            {Keys.A, new MovementCommand{EntityID = entityId, MovementVector = new Vector2(-1, 0)}},
            {Keys.S, new MovementCommand{EntityID = entityId, MovementVector = new Vector2(0, 1)}},
            {Keys.D, new MovementCommand{EntityID = entityId, MovementVector = new Vector2(1, 0)}},
        };
    }
    
    public void Process()
    {
        var keys = Keyboard.GetState().GetPressedKeys();
        var commands = new List<MovementCommand>();

        var length = keys.Length;
        for (int i = 0; i < length; i++)
        {
            if (keyCommandMap.TryGetValue(keys[i], out var command))
            {
                commands.Add(command);
            }
        }

        var count = commands.Count;
        for (int i = 0; i < count; i++)
        {
            ParseInputToCommand(commands[i]);
        }
    }
    private void ParseInputToCommand(MovementCommand command)
    {
        CommandCreated?.Invoke(this, command);
    }
}