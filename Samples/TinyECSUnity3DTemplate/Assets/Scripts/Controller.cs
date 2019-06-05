using UnityEngine;
using TinyECS.Impls;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;


public class Controller: MonoBehaviour
{
    protected IWorldContext  mWorldContext;

    protected ISystemManager mSystemManager;

    private void Awake()
    {
        mWorldContext = new WorldContextFactory().CreateNewWorldInstance();

        mSystemManager = new SystemManager(mWorldContext);

        WorldContextsManagerUtils.CreateWorldContextManager(mWorldContext, "WorldContextManager_System");
        SystemManagerObserverUtils.CreateSystemManagerObserver(mSystemManager, "SystemManagerObserver_System");

        mSystemManager.Init();
    }

    private void Update()
    {
        mSystemManager.Update(Time.deltaTime);
    }
}
