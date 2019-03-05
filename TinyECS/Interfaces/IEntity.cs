namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface IEntity
    /// 
    /// The interface describes a functionality of an entity, which is a container
    /// of components. Actually it doesn't contain them
    /// </summary>

    public interface IEntity
    {
        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>
        /// <returns>A component's value</returns>

        T AddComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// The method replaces existing component's value 
        /// </summary>
        /// <typeparam name="T">A type of a component that should be updated</typeparam>

        void ReplaceComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        void RemoveComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// The property returns an identifier of an entity
        /// </summary>

        uint Id { get; }
    }
}
