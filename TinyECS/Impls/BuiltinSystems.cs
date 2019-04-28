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
                worldContext.DestroyEntity(disposableEntities[i]);
            }
        }
    }
}
