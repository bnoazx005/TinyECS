namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface IEntityManager
    /// 
    /// The interface describes a functionality of an entity manager that
    /// is used by a world's instance for entities creation
    /// </summary>

    public interface IEntityManager
    {
        /// <summary>
        /// The method creates a new entity with a given name if it was specified
        /// </summary>
        /// <param name="name">An optional parameter that specifies a name of an entity</param>
        /// <returns>A reference to an entity</returns>

        IEntity CreateEntity(string name = null);

        /// <summary>
        /// The method destroy an entity with a given identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>The method returns true if the entity was successfully destroyed and false in other cases</returns>

        bool DestroyEntity(uint entityId);

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>

        void AddComponent<T>(uint entityId, T componentInitializer = default(T)) where T : struct, IComponent;

        /// <summary>
        /// The method replaces existing component's value 
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be updated</typeparam>

        void ReplaceComponent<T>(uint entityId) where T : struct, IComponent;

        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        void RemoveComponent<T>(uint entityId) where T : struct, IComponent;
    }
}
