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
        /// <typeparam name="T">A type of a component that should be attached</typeparam>
        /// <returns>A component's value</returns>

        T AddComponent<T>(uint entityId) where T : struct, IComponent;

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
