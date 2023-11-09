using System;

namespace MonoGameNetworking.Commands;

public interface ICommand
{
    public Guid EntityID { get; set; }
}