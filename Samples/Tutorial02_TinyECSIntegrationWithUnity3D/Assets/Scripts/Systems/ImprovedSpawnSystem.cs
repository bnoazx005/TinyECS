using System.Collections.Generic;
using TinyECS.Impls;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Interfaces;
using UnityEngine;

/// <summary>
/// This class will use all the power that TinyECS provides
/// </summary>

public class ImprovedSpawnSystem : BaseReactiveSystem
{
    protected IWorldContext      mWorldContext;

    protected GameObject         mPrefab;

    protected IGameObjectFactory mFactory;

    public ImprovedSpawnSystem(IWorldContext worldContext, GameObject prefab, IGameObjectFactory factory)
    {
        mWorldContext = worldContext;

        mPrefab = prefab;

        mFactory = factory;
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
            mFactory.Spawn(mPrefab, entities[i].GetComponent<TClickedComponent>().mWorldPosition,
                           Quaternion.identity, null);
        }
    }
}