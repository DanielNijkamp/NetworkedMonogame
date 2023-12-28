namespace ECS;

public sealed class World
{
    //key-value pair storage of entity and the components, for per-entity processing
    public Dictionary<Guid, IComponent[]> entities = new();
    //a per-component type-instance map where we can retrieve all the component of a certain type.
    //used by systems to iterate strictly by component instead of retrieving the component by entity
    private Dictionary<Type, List<IComponent>> componentsMap = new();


    public KeyValuePair<Guid, IComponent[]> CreateEntity(params IComponent[] components)
    {
        var UUID = Guid.NewGuid();
        var kvp = new KeyValuePair<Guid, IComponent[]>(UUID, components);
        entities.Add(kvp.Key, kvp.Value);

        var length = components.Length;
        for (int i = 0; i < length; i++)
        {
            var componentMapEntry = componentsMap[components[i].GetType()];
          
            componentMapEntry ??= new List<IComponent>(); //instantiate new if null

            componentMapEntry.Add(components[i]);
        }
        return kvp;
    }
    
    public KeyValuePair<Guid, IComponent[]> CreateEntity(Guid ID, params IComponent[] components)
    {
        var kvp = new KeyValuePair<Guid, IComponent[]>(ID, components);
        entities.Add(kvp.Key, kvp.Value);
        
        var length = components.Length;
        for (int i = 0; i < length; i++)
        {
            
            var componentType = components[i].GetType();
            
            //check if type-instance mapping exists and create one if not
            if (!componentsMap.ContainsKey(componentType))
            {
                componentsMap.TryAdd(componentType, new List<IComponent>());
            }
            //create the instance list if it does not exist already
            var componentOfType = componentsMap[componentType] ??= new List<IComponent>(); 
            componentOfType.Add(components[i]);
        }
        return kvp;
    }
    
    
    public List<IComponent> GetComponentsOfType<T>() where T : IComponent
    {
        return componentsMap.TryGetValue(typeof(T), out var result) ? result : null;
    }

    public List<List<IComponent>> GetComponentForTypes(params Type[] types)
    {
        var result = new List<List<IComponent>>();
        foreach (var type in types)
        {
            if(componentsMap.TryGetValue(type, out var arr))
            {
                result.Add(arr);
            } 
        }
        return result;
    }
    public List<IComponent>[] GetComponentForTypesImmutable(params Type[] types)
    {
        var result = new List<List<IComponent>>();
        foreach (var type in types)
        {
            if(componentsMap.TryGetValue(type, out var arr))
            {
                result.Add(arr);
            } 
        }
        return result.ToArray();
    }
    
    public IComponent[] GetComponentsOfTypeImmutable<T>() where T : IComponent
    {
        return componentsMap.TryGetValue(typeof(T), out var result) ? result.ToArray() : null;
    }
}