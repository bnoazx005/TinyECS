using TinyECS.Interfaces;
using TinyECSUnityIntegration.Interfaces;
using UnityEngine;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// struct TOnViewWaitForInitEventComponent
    /// 
    /// The structure describes a parameters of an event which is generated with BaseView
    /// when some instance of it has awaken and need to be registered
    /// </summary>

    public struct TOnViewWaitForInitEventComponent: IComponent
    {
        public BaseView mView;
    }


    /// <summary>
    /// struct TViewComponent
    /// 
    /// The structure contains a reference to a view which is linked to the entity
    /// </summary>

    public struct TViewComponent: IComponent
    {
        public BaseView mView;
    }


    /// <summary>
    /// class BaseView
    /// 
    /// The class is a base implementation of views for all that will be used to interact with a ECS model
    /// </summary>

    public abstract class BaseView: MonoBehaviour, IView
    {
        protected IWorldContext mWorldContext;

        private IEventManager   mEventManager;

        protected EntityId      mLinkedEntityId;

        /// <summary>
        /// The method prepares the view for initialization step
        /// </summary>
        /// <param name="worldContext">A reference to IWorldContext implementation</param>

        public void PreInit(IWorldContext worldContext)
        {
            WorldContext = worldContext;

            // create a new event which is an entity with attached component to inform system to register this view in the world's context
            IEntity registerViewRequestEntity = worldContext.GetEntityById(worldContext.CreateEntity());

            registerViewRequestEntity.AddComponent(new TOnViewWaitForInitEventComponent { mView = this });
        }

        /// <summary>
        /// The method links current view's instance with a given entity's identifier
        /// </summary>
        /// <param name="entityId">An integral identifier which represents some existing entity</param>

        public virtual void Link(EntityId entityId)
        {
            mLinkedEntityId = entityId;

            RegisterSubscriptions(_eventManager, entityId);
        }

        /// <summary>
        /// The method is used to provide a single place where all subscriptions are made
        /// </summary>
        /// <param name="eventManager">A reference to IEventManager implementation</param>
        /// <param name="entityId">Entity's identifier</param>

        public abstract void RegisterSubscriptions(IEventManager eventManager, EntityId entityId);

        /// <summary>
        /// The property returns a reference to IWorldContext which contains the entity that's linked
        /// to the view
        /// </summary>

        public IWorldContext WorldContext { get => mWorldContext; set => mWorldContext = value; }
        
        /// <summary>
        /// The property returns an identifier of the linked entity
        /// </summary>

        public EntityId LinkedEntityId => mLinkedEntityId;

        protected IEventManager _eventManager
        {
            get
            {
                if (mEventManager == null)
                {
                    mEventManager = mWorldContext?.EventManager;
                }

                return mEventManager;
            }
        }
    }
}
