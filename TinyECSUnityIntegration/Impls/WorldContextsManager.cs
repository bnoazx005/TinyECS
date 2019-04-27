using TinyECS.Interfaces;
using UnityEngine;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class WorldContextsManager
    /// 
    /// The class represents a manager that provides references to existing worlds contexts.
    /// NOTE: For now we support the only world context
    /// </summary>

    public class WorldContextsManager: MonoBehaviour
    {
        protected IWorldContext mWorldContext;

        public IWorldContext WorldContext { get => mWorldContext; set => mWorldContext = value; }
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

            return worldContextsManager;
        }
    }
}
