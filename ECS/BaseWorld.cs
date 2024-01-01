namespace ECS;

public class BaseWorld
{
    /// <summary>
    ///  key-value pair storage of entity and the components, for per-entity processing
    /// </summary>
    private Dictionary<Guid, IComponent[]> Entities = new();

    /// <summary>
    ///a per-component type-instance map where we can retrieve all the component of a certain type.
    ///used by systems to iterate strictly by component instead of retrieving the component via entity
    /// </summary>
    private Dictionary<Type, List<IComponent>> ComponentsMap = new();
    
    public virtual KeyValuePair<Guid, IComponent[]> CreateEntity(Guid ID, params IComponent[] components)
    {
        var kvp = new KeyValuePair<Guid, IComponent[]>(ID, components);
        Entities.Add(kvp.Key, kvp.Value);

        foreach (var component in components)
        {
            MapComponent(component);
        }
        return kvp;
    }

    public virtual bool DeleteEntity(Guid ID)
    {
        try
        {
            var components = Entities[ID];

            foreach (var component in components)
            {
                var type = component.GetType();
                ComponentsMap[type].Remove(component);
                ComponentsMap[type].TrimExcess();
            }
            Entities.Remove(ID);
            Entities.TrimExcess();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed Deletion of [{ID}]\\nl Message:[{e}]");
            return false;
        }
    }

    public KeyValuePair<Guid, IComponent[]> GetEntityById(Guid EntityID)
    {
        return Entities.FirstOrDefault(x => x.Key == EntityID);
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

    private void MapComponent(IComponent component)
    {
        var componentType = component.GetType();
        
        //check if type-instance mapping exists and create one if not
        //also create empty list so no nullExceptions
        if (!ComponentsMap.ContainsKey(componentType))
        {
            ComponentsMap.TryAdd(componentType, new List<IComponent>());
        }
        ComponentsMap[componentType].Add(component);
    }
}