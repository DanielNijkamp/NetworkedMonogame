using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameNetworking.ECS;

public class EntityManager
{
    private List<Entity> Entities = new List<Entity>();

    public Entity CreateEntity()
    {
        var entity = new Entity();
        Entities.Add(entity);
        return entity;
    }

    public Entity CreateEntity(Guid ID)
    {
        var entity = new Entity(ID);
        Entities.Add(entity);
        return entity;
    }

    public Entity GetEntityById(Guid ID)
    {
        return Entities.FirstOrDefault(e => e.EntityID == ID);
    }

    public IEnumerable<Entity> GetEntitiesWith<T>() where T : class, IComponent
    {
        return Entities.Where(e => e.HasComponent<T>());
    }

    public IEnumerable<Entity> GetEntitiesWith<T>(params T[] c)
    {
        return Entities.Where(e => e.HasComponents(c));
    }
}