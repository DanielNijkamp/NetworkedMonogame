using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using MonoGameNetworking.Authorization;
using MonoGameNetworking.Commands;
using MonoGameNetworking.Commands.MovementCommands;

namespace MonoGameNetworking;

public class KeyInputRouter : IKeyInputRouter
{
    private readonly ClientDataProvider dataProvider;
    public event EventHandler<ICommand> CommandCreated;
    private readonly Dictionary<Keys, ICommand> keyCommandMap;
    
    public KeyInputRouter(ClientDataProvider dataProvider)
    {
        keyCommandMap = new Dictionary<Keys, ICommand>
        {
            {Keys.W, new UpCommand(this.dataProvider.ClientID)},
            {Keys.A, new LeftCommand(this.dataProvider.ClientID)},
            {Keys.S, new DownCommand(this.dataProvider.ClientID)},
            {Keys.D, new RightCommand(this.dataProvider.ClientID)},
        };
        this.dataProvider = dataProvider;
    }
    
    public async Task ParseKeyboardState(KeyboardState state)
    {
        var ParsedCommands = state.GetPressedKeys().Select(key =>
        {
            keyCommandMap.TryGetValue(key, out ICommand command);
            return command;
        }).ToImmutableArray();
        
        
        var length = ParsedCommands.Length;
        for (int i = 0; i < length; i++)
        { 
            if (ParsedCommands[i] == null) continue;
            await ParseInputToCommand(ParsedCommands[i]);
        }
    }
    private async Task ParseInputToCommand(ICommand command)
    {
        CommandCreated?.Invoke(this, command);
    }

}