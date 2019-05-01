using TinyECS.Interfaces;
using UnityEngine;


namespace TinyECSUnityIntegration.Interfaces
{
    /// <summary>
    /// interface IGameObjectFactory
    /// 
    /// The interface represents a factory of objects of GameObject type. Use 
    /// the implementation of this interface should be used as a replacement
    /// for GameObject.Instantiate invocations
    /// </summary>

    public interface IGameObjectFactory
    {
        /// <summary>
        /// The method instantiates a new instance of a given prefab
        /// </summary>
        /// <param name="prefab">A reference to a prefab that should be created dynamically</param>
        /// <param name="position">A position of a new object</param>
        /// <param name="rotation">An orientation of an object in a space</param>
        /// <param name="parent">A parent of a new created object</param>
        /// <returns>A reference to a new created entity which is attached to a new created GameObject</returns>

        IEntity Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent);
    }
}
