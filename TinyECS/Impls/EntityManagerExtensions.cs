using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// static class EntityManagerExtensions
    /// 
    /// The class contains helper methods for EntityManager
    /// </summary>

    public static class EntityManagerExtensions
    {
        /// <summary>
        /// The method creates a new entity with the only component that's already attached to it.
        /// The component is TDisposableComponent. All entities that have that component are destroyed
        /// at an end of a frame with a special system.
        /// </summary>
        /// <param name="entityManager">A reference to IEntityManager implementation</param>
        /// <param name="name">A name of an entity (optional)</param>
        /// <returns>The method returns an identifier of created entity</returns>

        public static uint CreateDisposableEntity(this IEntityManager entityManager, string name = null)
        {
            IEntity entity = entityManager.CreateEntity(name);

            entity.AddComponent<TDisposableComponent>();

            return entity.Id;
        }

        /// <summary>
        /// The method creates a new entity with the only component that's already attached to it.
        /// The component is TDisposableComponent. All entities that have that component are destroyed
        /// at an end of a frame with a special system.
        /// </summary>
        /// <param name="worldContext">A reference to IWorldContext implementation</param>
        /// <param name="name">A name of an entity (optional)</param>
        /// <returns>The method returns an identifier of created entity</returns>

        public static uint CreateDisposableEntity(this IWorldContext worldContext, string name = null)
        {
            IEntity entity = worldContext.GetEntityById(worldContext.CreateEntity(name));

            entity.AddComponent<TDisposableComponent>();

            return entity.Id;
        }
    }
}
