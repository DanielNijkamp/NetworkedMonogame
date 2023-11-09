using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using MonoGameNetworking.Commands;

namespace MonoGameNetworking;

public interface IKeyInputRouter
{
    public event EventHandler<ICommand> CommandCreated;
    public Task ParseKeyboardState(KeyboardState state);
}