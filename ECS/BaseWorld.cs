namespace ECS;

public class BaseWorld
{
    /// <summary>
    ///  key-value pair storage of entity and the components, for per-entity processing
    /// </summary>
    public Dictionary<Guid, IComponent[]> Entities;

    /// <summary>
    ///a per-component type-instance map where we can retrieve all the component of a certain type.
    ///used by systems to iterate strictly by component instead of retrieving the component via entity
    /// </summary>
    private Dictionary<Type, List<IComponent>> ComponentsMap;

    public virtual KeyValuePair<Guid, IComponent[]> CreateEntity(params IComponent[] components)
    {
        var UUID = Guid.NewGuid();
        var kvp = new KeyValuePair<Guid, IComponent[]>(UUID, components);
        Entities.Add(kvp.Key, kvp.Value);

        var length = components.Length;
        for (int i = 0; i < length; i++)
        {
            var componentType = components[i].GetType();
            
            //check if type-instance mapping exists and create one if not
            //also create empty list so no nullExceptions
            if (!ComponentsMap.ContainsKey(componentType))
            {
                ComponentsMap.TryAdd(componentType, new List<IComponent>());
            }
            ComponentsMap[componentType].Add(components[i]);
        }
        return kvp;
    }

    public virtual KeyValuePair<Guid, IComponent[]> CreateEntity(Guid ID, params IComponent[] components)
    {
        var kvp = new KeyValuePair<Guid, IComponent[]>(ID, components);
        Entities.Add(kvp.Key, kvp.Value);
        
        var length = components.Length;
        for (int i = 0; i < length; i++)
        {
            var componentType = components[i].GetType();
            
            //check if type-instance mapping exists and create one if not
            //also create empty list so no nullExceptions
            if (!ComponentsMap.ContainsKey(componentType))
            {
                ComponentsMap.TryAdd(componentType, new List<IComponent>());
            }
            ComponentsMap[componentType].Add(components[i]);
        }
        return kvp;
    }

    public virtual List<IComponent> GetComponentsOfType<T>() where T : IComponent
    {
        return (ComponentsMap.TryGetValue(typeof(T), out var result) ? result : null)!;
    }

    public virtual List<List<IComponent>> GetComponentForTypes(params Type[] types)
    {
        var result = new List<List<IComponent>>();
        foreach (var type in types)
        {
            if(ComponentsMap.TryGetValue(type, out var arr))
            {
                result.Add(arr);
            } 
        }
        return result;
    }

    public virtual List<IComponent>[] GetComponentForTypesImmutable(params Type[] types)
    {
        var result = new List<List<IComponent>>();
        foreach (var type in types)
        {
            if(ComponentsMap.TryGetValue(type, out var arr))
            {
                result.Add(arr);
            } 
        }
        return result.ToArray();
    }

    public virtual IComponent[] GetComponentsOfTypeImmutable<T>() where T : IComponent
    {
        return ComponentsMap.TryGetValue(typeof(T), out var result) ? result.ToArray() : null;
    }
}