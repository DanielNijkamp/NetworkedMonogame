namespace ECS;

public class World
{
    /// <summary>
    ///  key-value pair storage of entity and the components, for per-entity processing
    /// </summary>
    private Dictionary<Guid, Component[]> Entities = new();

    /// <summary>
    ///a per-component type-instance map where we can retrieve all the component of a certain type.
    ///used by systems to iterate strictly by component instead of retrieving the component via entity
    /// </summary>
    private Dictionary<Type, List<Component>> ComponentsMap = new();
    
    public virtual KeyValuePair<Guid, Component[]> CreateEntity(Guid ID, params Component[] components)
    {
        var kvp = new KeyValuePair<Guid, Component[]>(ID, components);
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

    public KeyValuePair<Guid, Component[]> GetEntityById(Guid EntityID)
    {
        return Entities.FirstOrDefault(x => x.Key == EntityID);
    }

    public virtual List<T> GetComponentsOfType<T>() where T : Component
    {
        return (ComponentsMap.TryGetValue(typeof(T), out var result) ? result as List<T> : null)!;
    }
    

    //function could be useful to get multiple components in 1 function call but its too janky to work with now since
    //wanting to use a component of a certain types means you need to cast it everytime which is dumb
    /*public virtual Dictionary<Type, T[]> GetComponentForTypesImmutable<T>(params Type[] types) where T : Component
    {
        var result = new Dictionary<Type, T[]>();
        foreach (var type in types)
        {
            if (ComponentsMap.TryGetValue(type, out var arr))
            {
                result[type] = arr.Cast<T>().ToArray();
            }
        }
        return result;
    }*/
    public virtual T[] GetComponentsOfTypeImmutable<T>() where T : Component
    {
        return ComponentsMap.TryGetValue(typeof(T), out var result) ? result.OfType<T>().ToArray() : Array.Empty<T>();
    }

    private void MapComponent(Component component)
    {
        var componentType = component.GetType();
        
        //check if type-instance mapping exists and create one if not
        //also create empty list so no nullExceptions
        if (!ComponentsMap.ContainsKey(componentType))
        {
            ComponentsMap.TryAdd(componentType, new List<Component>());
        }
        ComponentsMap[componentType].Add(component);
    }

    public T GetComponentOfType<T>(Component[] components) where T : Component
    {
        return components.OfType<T>().FirstOrDefault()!;
    }
}