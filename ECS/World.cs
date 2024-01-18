namespace ECS;

public class World
{
    public EntityStorage storage { get; private set; } = new();
    
    public virtual void CreateEntity(Guid ID, params Component[] components)
    {
        storage.Entities.Add(ID, components);
        foreach (var component in components)
        {
            MapComponent(component);
        }
    }

    public virtual bool DeleteEntity(Guid ID)
    {
        try
        {
            var components = storage.Entities[ID];

            foreach (var component in components)
            {
                var typeName = component.GetType().AssemblyQualifiedName;
                storage.ComponentsMap[typeName].Remove(component);
                storage.ComponentsMap[typeName].TrimExcess();
            }
            storage.Entities.Remove(ID);
            storage.Entities.TrimExcess();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed Deletion of [{ID}]\\nl Message:[{e}]");
            return false;
        }
    }

    public Component[] GetComponentsFromEntity(Guid EntityID)
    {
        return storage.Entities.FirstOrDefault(x => x.Key == EntityID).Value;
    }

    public virtual List<T> GetComponentsOfType<T>() where T : Component
    {
        return (storage.ComponentsMap.TryGetValue(typeof(T).AssemblyQualifiedName, out var result) ? result as List<T> : null)!;
    }
    

    //function could be useful to get multiple components in 1 function call but its too janky to work with now since
    //wanting to use a component of certain types means you need to cast it everytime which is dumb
    
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
        return storage.ComponentsMap.TryGetValue(typeof(T).AssemblyQualifiedName, out var result) ? result.OfType<T>().ToArray() : Array.Empty<T>();
    }

    private void MapComponent<T>(T component) where T : Component
    {
        var componentName =  component.GetType().AssemblyQualifiedName;
        
        //check if type-instance mapping exists and create one if not
        //also create empty list so no nullExceptions
        if (!storage.ComponentsMap.ContainsKey(componentName))
        {
            storage.ComponentsMap.TryAdd(componentName, new List<Component>());
        }
        storage.ComponentsMap[componentName].Add(component);
    }

    public T GetComponentOfType<T>(Component[] components) where T : Component
    {
        return components.OfType<T>().FirstOrDefault()!;
    }

    public void Replicate(EntityStorage storage)
    {
        this.storage = storage;
        storage.RebuildComponentsMap();
    }
}