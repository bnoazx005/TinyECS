using TinyECS.Interfaces;
using TinyECSUnityIntegration.Interfaces;
using UnityEngine;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class BaseView
    /// 
    /// The class is a base implementation of views for all that will be used to interact with a ECS model
    /// </summary>

    public abstract class BaseView: MonoBehaviour, IView
    {
        protected IWorldContext mWorldContext;

        private IEventManager   mEventManager;

        protected uint          mLinkedEntityId;

        /// <summary>
        /// The method links current view's instance with a given entity's identifier
        /// </summary>
        /// <param name="entityId">An integral identifier which represents some existing entity</param>

        public virtual void Link(uint entityId)
        {
            mLinkedEntityId = entityId;

            RegisterSubscriptions(_eventManager, entityId);
        }

        /// <summary>
        /// The method is used to provide a single place where all subscriptions are made
        /// </summary>
        /// <param name="eventManager">A reference to IEventManager implementation</param>
        /// <param name="entityId">Entity's identifier</param>

        public abstract void RegisterSubscriptions(IEventManager eventManager, uint entityId);

        /// <summary>
        /// The property returns a reference to IWorldContext which contains the entity that's linked
        /// to the view
        /// </summary>

        public IWorldContext WorldContext { get => mWorldContext; set => mWorldContext = value; }
        
        /// <summary>
        /// The property returns an identifier of the linked entity
        /// </summary>

        public uint LinkedEntityId => mLinkedEntityId;

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
