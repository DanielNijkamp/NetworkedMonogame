using MessagePack;

namespace ECS;

[MessagePackObject(keyAsPropertyName:true)]
public class EntityStorage
{
    /// <summary>
    ///  key-value pair storage of entity and the components, for per-entity processing
    /// </summary>
    public Dictionary<Guid, Component[]> Entities { get; set; }= new();

    /// <summary>
    ///a per-component type-instance map where we can retrieve all the component of a certain type.
    ///used by systems to iterate strictly by component instead of retrieving the component via entity
    /// </summary>
    public Dictionary<Type, List<Component>> ComponentsMap { get; set; }= new();
}