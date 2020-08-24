using TinyECS.Impls;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;
using UnityEngine;


public class TestController : MonoBehaviour
{
    protected IWorldContext  mWorldContext;

    protected ISystemManager mSystemManager;

    protected void Awake()
    {
        mWorldContext = new WorldContextFactory().CreateNewWorldInstance();

        mSystemManager = new SystemManager(mWorldContext);

        WorldContextsManagerUtils.CreateWorldContextManager(mWorldContext, "WorldsContextsManager_System");
        SystemManagerObserverUtils.CreateSystemManagerObserver(mSystemManager, "SystemManagerObserver_System");

        mSystemManager.RegisterSystem(new RegisterViewSystem(mWorldContext));
        
        mSystemManager.Init();

    }

    protected void Update() => mSystemManager.Update(Time.deltaTime);

    public IWorldContext WorldContext => mWorldContext;

    public static TestController Create()
    {
        GameObject controllerObject = new GameObject("Controller");
        controllerObject.AddComponent<TestController>();

        return controllerObject.GetComponent<TestController>();
    }
}
