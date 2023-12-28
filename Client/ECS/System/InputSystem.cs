using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Commands.MovementCommands;
using Microsoft.Xna.Framework.Input;

namespace MonoGameNetworking.ECS.System;

public class InputSystem
{
    public event EventHandler<IMovementCommand> CommandCreated;
    
    private readonly Dictionary<Keys, IMovementCommand> keyCommandMap;

    public InputSystem(Guid EntityID)
    {
        keyCommandMap = new Dictionary<Keys, IMovementCommand>
        {
            {Keys.W, new UpCommand { EntityID = EntityID}},
            {Keys.A, new LeftCommand { EntityID = EntityID}},
            {Keys.S, new DownCommand { EntityID = EntityID}},
            {Keys.D, new RightCommand { EntityID = EntityID}},
        };
    }
    
    public void Process()
    {
        var parsedCommands = Keyboard.GetState().GetPressedKeys()
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
    private void ParseInputToCommand(IMovementCommand command)
    {
        CommandCreated?.Invoke(this, command);
    }
}