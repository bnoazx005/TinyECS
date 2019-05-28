using System;
using TinyECS.Interfaces;
using UnityEngine;



namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class EntityObserver
    /// 
    /// The class is used to provide the debug information about some entity within Unity editor
    /// </summary>

    public class EntityObserver: MonoBehaviour
    {
        public uint             mEntityId;

        protected IWorldContext mWorldContext;

        /// <summary>
        /// The method initializes current state of the observer
        /// </summary>
        /// <param name="worldContext">A reference to a world context from which the entity will be retrieved</param>
        /// <param name="entityId">An identifier of an entity</param>

        public void Init(IWorldContext worldContext, uint entityId)
        {
            mWorldContext = worldContext ?? throw new ArgumentNullException("worldContext");

            mEntityId = entityId;
        }

        public IEntity Entity => mWorldContext?.GetEntityById(mEntityId);
    }
}
