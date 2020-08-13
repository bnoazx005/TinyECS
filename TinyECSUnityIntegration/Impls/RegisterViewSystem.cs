using System.Collections.Generic;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class RegisterViewSystem
    /// 
    /// The class represents a reactive system that links created entities with their views
    /// in Unity3D side
    /// </summary>

    public class RegisterViewSystem : BaseReactiveSystem
    {
        protected IWorldContext mWorldContext;

        public RegisterViewSystem(IWorldContext worldContext)
        {
            mWorldContext = worldContext;
        }

        public override bool Filter(IEntity entity)
        {
            return entity.HasComponent<TOnViewWaitForInitEventComponent>();
        }

        public override void Update(List<IEntity> entities, float deltaTime)
        {
            IEntity currEntity = null;

            TOnViewWaitForInitEventComponent currRegisterViewRequestComponent;

            IEventManager eventManager = mWorldContext?.EventManager;

            for (int i = 0; i < entities.Count; ++i)
            {
                currEntity = entities[i];

                currRegisterViewRequestComponent = currEntity.GetComponent<TOnViewWaitForInitEventComponent>();

                currRegisterViewRequestComponent.mView?.Link(currEntity.Id);

                currEntity.AddComponent(new TViewComponent { mView = currRegisterViewRequestComponent.mView });

                currEntity.RemoveComponent<TOnViewWaitForInitEventComponent>();
            }
        }
    }
}
