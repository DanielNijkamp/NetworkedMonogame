using System;

namespace MonoGameNetworking.Authorization;

public class ClientDataProvider
{
    public uint ClientID { get; private set; }

    public ClientDataProvider()
    {
        ClientID = (uint)new Random().Next();
    }
}