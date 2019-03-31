namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface IComponentManager
    /// 
    /// The interface describes a functionality that a component manager should
    /// provide to a end-user
    /// </summary>

    public interface IComponentManager
    {
        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>

        void AddComponent<T>(uint entityId, T componentInitializer = default(T)) where T : struct, IComponent;

        /// <summary>
        /// The method returns a component of a given type if it belongs to
        /// the specified entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a component of a given type if it belongs to
        /// the specified entity</returns>

        T GetComponent<T>(uint entityId) where T : struct, IComponent;
        
        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        void RemoveComponent<T>(uint entityId) where T : struct, IComponent;

        /// <summary>
        /// The method removes all components that are attached to the entity with the specified identifier
        /// <param name="entityId">Entity's identifier</param>
        /// </summary>

        void RemoveAllComponents(uint entityId);
    }
}
