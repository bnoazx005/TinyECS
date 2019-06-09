using UnityEngine;
using TinyECS.Impls;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;


public class Controller: MonoBehaviour
{
    public GameObject        mPrefab;

    protected IWorldContext  mWorldContext;

    protected ISystemManager mSystemManager;

    private void Awake()
    {
        mWorldContext = new WorldContextFactory().CreateNewWorldInstance();

        mSystemManager = new SystemManager(mWorldContext);

        WorldContextsManagerUtils.CreateWorldContextManager(mWorldContext, "WorldContextManager_System");
        SystemManagerObserverUtils.CreateSystemManagerObserver(mSystemManager, "SystemManagerObserver_System");

        // register our systems here
        mSystemManager.RegisterUpdateSystem(new InputSystem(mWorldContext, Camera.main));
        mSystemManager.RegisterReactiveSystem(new ImprovedSpawnSystem(mWorldContext, mPrefab, new GameObjectFactory(mWorldContext)));
        mSystemManager.RegisterReactiveSystem(new RegisterViewSystem(mWorldContext));
        mSystemManager.RegisterUpdateSystem(new RotatingCubesSystem(mWorldContext));

        mSystemManager.Init();
    }

    private void Update()
    {
        mSystemManager.Update(Time.deltaTime);
    }
}
