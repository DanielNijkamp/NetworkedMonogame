using MessagePack;

namespace ECS;

[MessagePackObject(keyAsPropertyName:true)]
public class EntityStorage
{
    /// <summary>
    ///  key-value pair storage of entity and the components, for per-entity processing
    /// </summary>
    public Dictionary<Guid, Component[]> Entities { get; set; } = new();

    /// <summary>
    ///a per-component type-instance map where we can retrieve all the component of a certain type.
    ///used by systems to iterate strictly by component instead of retrieving the component via entity
    /// </summary>
    /// Was a Type, Component map but now is string, Component map since Type is not serializable.
    /// custom serialization also failed so something to look into in the future
    public Dictionary<string, List<Component>> ComponentsMap { get; set; }= new();


    //rebuild component references because serialization nukes them
    public void RebuildComponentsMap()
    {
        ComponentsMap.Clear();
        foreach (var entity in Entities)
        {
            foreach (var component in entity.Value)
            {
                var componentName = component.GetType().AssemblyQualifiedName;

                // If this component type hasn't been seen before, add it to the map
                if (!ComponentsMap.ContainsKey(componentName))
                {
                    ComponentsMap[componentName] = new List<Component>();
                }

                // Add this component to the map
                ComponentsMap[componentName].Add(component);
            }
        }
    }
}