#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MonoGameNetworking.ECS;

public class Entity
{
    public Entity()
    {
        EntityID = Guid.NewGuid();
    }

    public Entity(Guid ID)
    {
        EntityID = ID;
    }
    
    public Guid EntityID { get; private set; }

    private List<IComponent> components = new();
    
    public void AddComponent(IComponent component)
    {
        this.components.Add(component);
    }

    public T GetComponent<T>() where T : class , IComponent
    {
        return components.OfType<T>().FirstOrDefault();
    }
    public bool GetComponent<T>(out T value) where T : class , IComponent
    {
        value = components.OfType<T>().FirstOrDefault();
        return value != null;
    }
    public bool HasComponent<T>() where T: class , IComponent
    {
        return components.Any(c => c is T);
    }

    public bool HasComponents<T>(params T[] components)
    {
        return components.Any(c => c is T);
    }
}