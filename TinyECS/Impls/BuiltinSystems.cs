using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// static class BuiltinSystems
    /// 
    /// The static class contains a bunch of builtin systems that are represented as static functions
    /// </summary>

    public static class BuiltinSystems
    {
        /// <summary>
        /// The system is an update one that destroys all disposable entities at an end of a frame
        /// </summary>
        /// <param name="worldContext"></param>
        /// <param name="dt"></param>

        public static void DisposableEntitiesCollectorSystem(IWorldContext worldContext, float dt)
        {
            var disposableEntities = worldContext.GetEntitiesWithAll(typeof(TDisposableComponent));

            for (int i = 0; i < disposableEntities.Count; ++i)
            {
                IEntity currEntity = worldContext.GetEntityById(disposableEntities[i]);
                if (currEntity.HasComponent<TEntityLifetimeComponent>())
                {
                    TEntityLifetimeComponent lifetimeData = currEntity.GetComponent<TEntityLifetimeComponent>();
                    currEntity.AddComponent(new TEntityLifetimeComponent { mCounter = lifetimeData.mCounter - 1 });

                    if (lifetimeData.mCounter > 0)
                    {
                        continue;
                    }
                }

                worldContext.DestroyEntity(disposableEntities[i]);
            }
        }
    }
}
