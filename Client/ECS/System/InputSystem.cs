using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Commands.EntityCommands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameNetworking.ECS.System;

public class InputSystem
{
    public event EventHandler<MovementCommand>? CommandCreated;
    
    private readonly Dictionary<Keys, MovementCommand> keyCommandMap;

    public InputSystem(Guid entityId)
    {
        keyCommandMap = new Dictionary<Keys, MovementCommand>
        {
            {Keys.W, new MovementCommand{EntityID = entityId,  MovementVector = new Vector2(0, -1)}},
            {Keys.A, new MovementCommand{EntityID = entityId, MovementVector = new Vector2(-1, 0)}},
            {Keys.S, new MovementCommand{EntityID = entityId, MovementVector = new Vector2(0, 1)}},
            {Keys.D, new MovementCommand{EntityID = entityId, MovementVector = new Vector2(1, 0)}},
        };
    }
    
    public void Process()
    {
        var parsedCommands = Keyboard.GetState().GetPressedKeys()
            .AsParallel()
            .Where(keyCommandMap.ContainsKey)
            .Select(key => keyCommandMap[key])
            .ToImmutableArray();
        
        var length = parsedCommands.Length;
        for (int i = 0; i < length; i++)
        { 
            if (parsedCommands[i] == null) continue;
            ParseInputToCommand(parsedCommands[i]);
        }
    }
    private void ParseInputToCommand(MovementCommand command)
    {
        CommandCreated?.Invoke(this, command);
    }
}