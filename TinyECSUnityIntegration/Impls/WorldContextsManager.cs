using TinyECS.Interfaces;
using TinyECS.Impls;
using UnityEngine;
using TinyECSUnityIntegration.Interfaces;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class WorldContextsManager
    /// 
    /// The class represents a manager that provides references to existing worlds contexts.
    /// NOTE: For now we support the only world context
    /// </summary>

    public class WorldContextsManager: MonoBehaviour, IEventListener<TNewEntityCreatedEvent>, IEventListener<TEntityDestroyedEvent>
    {
        protected IWorldContext mWorldContext;

        protected uint          mNewEntityCreatedListenerId = uint.MaxValue;

        protected uint          mEntityRemovedListenerId = uint.MaxValue;

        protected Transform     mCachedTransform;

        public void PrepareViews()
        {
            IDependencyInjector[] viewsInjectors = GameObject.FindObjectsOfType<DependencyInjector>();
            for (int i = 0; i < viewsInjectors.Length; ++i)
            {
                viewsInjectors[i]?.Init();
            }
        }

        public void OnEvent(TNewEntityCreatedEvent eventData)
        {
            IEntity entity = mWorldContext.GetEntityById(eventData.mEntityId);

#if false//DEBUG
            Debug.Log($"[WorldContextsManager] A new entity ({entity.Name}) was created");
#endif

            GameObject entityGO = _createEntityGOView(entity, _cachedTransform);

            // initializes the observer of the entity
            EntityObserver entityObserver = entityGO.AddComponent<EntityObserver>();

            entityObserver.Init(mWorldContext, entity.Id);
        }

        public void OnEvent(TEntityDestroyedEvent eventData)
        {
#if DEBUG
            Debug.Log($"[WorldContextsManager] Entity ({eventData.mEntityName}) was destroyed");
#endif

            GameObject entityGO = GameObject.Find(eventData.mEntityName);

            GameObject.DestroyImmediate(entityGO);
        }

        protected GameObject _createEntityGOView(IEntity entity, Transform parent)
        {
            GameObject entityGO = new GameObject(entity.Name);

            Transform entityGOTransform = entityGO.GetComponent<Transform>();

            entityGOTransform.parent = parent;

            return entityGO;
        }

        public IWorldContext WorldContext
        {
            get
            {
                return mWorldContext;
            }

            set
            {
                mWorldContext = value;

                var eventManager = mWorldContext.EventManager;

                // unsubscribe previous listeners
                if (mNewEntityCreatedListenerId != uint.MaxValue)
                {
                    eventManager.Unsubscribe(mNewEntityCreatedListenerId);
                }

                if (mEntityRemovedListenerId != uint.MaxValue)
                {
                    eventManager.Unsubscribe(mEntityRemovedListenerId);
                }

                mNewEntityCreatedListenerId = eventManager.Subscribe<TNewEntityCreatedEvent>(this);
                mEntityRemovedListenerId    = eventManager.Subscribe<TEntityDestroyedEvent>(this);
            }
        }

        protected Transform _cachedTransform
        {
            get
            {
                if (mCachedTransform == null)
                {
                    mCachedTransform = GetComponent<Transform>();
                }

                return mCachedTransform;
            }
        }
    }


    /// <summary>
    /// class WorldContextsManagerUtils
    /// 
    /// The static class contains helper methods to initialize WorldContextsManager
    /// </summary>

    public static class WorldContextsManagerUtils
    {
        /// <summary>
        /// The extension method returns a new allocated manager of world contexts
        /// </summary>
        /// <param name="worldContext"></param>
        /// <param name="name">A name of a game object that will have WorldContextsManager component</param>
        /// <returns>The extension method returns a new allocated manager of world contexts</returns>

        public static WorldContextsManager CreateWorldContextManager(this IWorldContext worldContext, string name = null)
        {
            GameObject worldContextsManagerGO = new GameObject(name);

            WorldContextsManager worldContextsManager = worldContextsManagerGO.AddComponent<WorldContextsManager>();

            worldContextsManager.WorldContext = worldContext;

            worldContextsManager.PrepareViews();

            return worldContextsManager;
        }

        public static IEntity CreateAndGetEntity(this IWorldContext worldContext, string name = null)
        {
            return worldContext.GetEntityById(worldContext.CreateEntity(name));
        }
    }
}
