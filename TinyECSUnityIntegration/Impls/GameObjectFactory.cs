using System;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Interfaces;
using UnityEngine;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class GameObjectFactory
    /// 
    /// The class is an implementation of IGameObjectFactory that should be used
    /// as a replacement of GameObject.Instantiate
    /// </summary>

    public class GameObjectFactory: IGameObjectFactory
    {
        protected IWorldContext mWorldContext;

        public GameObjectFactory(IWorldContext worldContext)
        {
            mWorldContext = worldContext ?? throw new ArgumentNullException("worldContext");
        }

        /// <summary>
        /// The method instantiates a new instance of a given prefab
        /// </summary>
        /// <param name="prefab">A reference to a prefab that should be created dynamically</param>
        /// <param name="position">A position of a new object</param>
        /// <param name="rotation">An orientation of an object in a space</param>
        /// <param name="parent">A parent of a new created object</param>
        /// <returns>A reference to a new created entity which is attached to a new created GameObject</returns>

        public IEntity Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObjectInstance = GameObject.Instantiate(prefab, position, rotation, parent);

            IView viewComponent = gameObjectInstance.GetComponent<IView>() ?? throw new NullReferenceException();

            uint linkedEntityId = mWorldContext.CreateEntity(/*gameObjectInstance.name*/);

            IEntity linkedEntity = mWorldContext.GetEntityById(linkedEntityId);

            viewComponent.WorldContext = mWorldContext;

            linkedEntity.AddComponent(new TViewComponent { mView = viewComponent as BaseView });

            viewComponent.Link(linkedEntityId);
            
            return linkedEntity;
        }
    }
}
