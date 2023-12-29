using ECS;

namespace Server;


public class NetworkedWorld : BaseWorld
{
    //event called when entity is created on the server-side to broadcast to clients
    public EventHandler<KeyValuePair<Guid, IComponent[]>> OnEntityCreation;
    
    public override KeyValuePair<Guid, IComponent[]> CreateEntity(params IComponent[] components)
    {
        var result = base.CreateEntity(components);
        OnEntityCreation?.Invoke(this, result);
        return result;
    }

    public override KeyValuePair<Guid, IComponent[]> CreateEntity(Guid ID, params IComponent[] components)
    {
        var result = base.CreateEntity(ID,components);
        OnEntityCreation?.Invoke(this, result);
        return result;
    }
    
}