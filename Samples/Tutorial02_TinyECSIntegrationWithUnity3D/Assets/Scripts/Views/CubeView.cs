using TinyECS.Impls;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;


public class CubeView : BaseDynamicView, IEventListener<TComponentChangedEvent<TRotationComponent>>
{
    public override void RegisterSubscriptions(IEventManager eventManager, uint entityId)
    {
        IEntity linkedEntity = mWorldContext.GetEntityById(entityId);

        linkedEntity.AddComponent(new TRotatingCubeComponent { mSpeed = 5.0f });
        linkedEntity.AddComponent(new TRotationComponent { mRotation = transform.rotation });

        eventManager.Subscribe<TComponentChangedEvent<TRotationComponent>>(this);
    }

    public void OnEvent(TComponentChangedEvent<TRotationComponent> eventData)
    {
        transform.rotation = eventData.mValue.mRotation;
    }
}
