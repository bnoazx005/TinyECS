using TinyECS.Interfaces;
using UnityEngine;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class SystemManagerObserver
    /// 
    /// The class is used for integration of TinyECS with Unity3D and stores a reference to some
    /// ISystemManager implementation
    /// </summary>

    public class SystemManagerObserver: MonoBehaviour
    {
        protected ISystemManager mSystemManager;

        public ISystemManager SystemManager { get => mSystemManager; set => mSystemManager = value; }
    }


    /// <summary>
    /// class SystemManagerObserverUtils
    /// 
    /// The static class contains helper methods that are used wih SystemManagerObserver
    /// </summary>

    public static class SystemManagerObserverUtils
    {
        /// <summary>
        /// The method creates a new GameObject which is a representation of ISystemManager within Unity3D's scene
        /// </summary>
        /// <param name="systemManager"></param>
        /// <param name="name">A name of a game object that will have SystemManagerObserver component</param>
        /// <returns>The extension method returns a new allocated observer of ISystemManager type</returns>

        public static SystemManagerObserver CreateSystemManagerObserver(this ISystemManager systemManager, string name = null)
        {
            GameObject systemManagerObserverGO = new GameObject(name);

            SystemManagerObserver systemManagerObserver = systemManagerObserverGO.AddComponent<SystemManagerObserver>();

            systemManagerObserver.SystemManager = systemManager;

            return systemManagerObserver;
        }
    }
}
