using System.Collections.Generic;
using TinyECS.Impls;
using TinyECS.Interfaces;
using UnityEngine;

/// <summary>
/// This class is a type of a reactive system.
/// Reactive systems are executed when something within world's context has changed
/// and this update passes system's filter
/// </summary>

public class SpawnSystem : BaseReactiveSystem
{
    protected IWorldContext mWorldContext;

    protected GameObject    mPrefab;

    public SpawnSystem(IWorldContext worldContext, GameObject prefab)
    {
        mWorldContext = worldContext;

        mPrefab = prefab;
    }

    /// <summary>
    /// This method filters entities that were updated by someone. If method returns true
    /// the entity has passed the test, otherwise it's failed and will be skipped
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>

    public override bool Filter(IEntity entity)
    {
        return entity.HasComponent<TClickComponent>() && entity.HasComponent<TClickedComponent>();
    }

    public override void Update(List<IEntity> entities, float deltaTime)
    {
        for (int i = 0; i < entities.Count; ++i)
        {
            GameObject.Instantiate(mPrefab, entities[i].GetComponent<TClickedComponent>().mWorldPosition,
                                   Quaternion.identity, null);
        }
    }
}