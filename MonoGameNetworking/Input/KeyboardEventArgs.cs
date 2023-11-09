using System;
using Microsoft.Xna.Framework.Input;

namespace MonoGameNetworking;

public class KeyboardEventArgs : EventArgs
{
    public Keys PressedKey { get; }
    
    public KeyboardEventArgs(Keys pressedKey)
    {
        PressedKey = pressedKey;
    }
}