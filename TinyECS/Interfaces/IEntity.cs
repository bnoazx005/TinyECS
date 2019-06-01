using System;

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
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>

        void AddComponent<T>(T componentInitializer = default(T)) where T : struct, IComponent;

        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        void RemoveComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// The method removes all components that are attached to the entity
        /// </summary>

        void RemoveAllComponents();
        
        /// <summary>
        /// The method returns a component of a given type if it belongs to
        /// the specified entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <returns>The method returns a component of a given type if it belongs to
        /// the specified entity</returns>

        T GetComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// The method checks up whether a given entity has specified component or not
        /// </summary>
        /// <typeparam name="T">A type of a component</typeparam>
        /// <returns>The method returns true if the entity has the given component, false in other cases</returns>

        bool HasComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// The method checks up whether a given entity has specified component or not
        /// </summary>
        /// <param name="componentType">A type of a component</param>
        /// <returns>The method returns true if the entity has the given component, false in other cases</returns>

        bool HasComponent(Type componentType);

        /// <summary>
        /// The method creates a new iterator which provides an ability to enumerate all components of the entity
        /// </summary>
        /// <returns>The method returns a reference to IComponentIterator that implements some iterative mechanism</returns>

        IComponentIterator GetComponentsIterator();

        /// <summary>
        /// The property returns an identifier of an entity
        /// </summary>

        uint Id { get; }

        /// <summary>
        /// The property returns a name of an entity
        /// </summary>

        string Name { get; }
    }
}
