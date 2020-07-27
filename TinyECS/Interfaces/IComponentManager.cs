﻿using System;

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
        /// The method is used to register the given entity within the internal data structure, but
        /// withou actual allocating memory for components
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>

        void RegisterEntity(EntityId entityId);

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>

        void AddComponent<T>(EntityId entityId, T componentInitializer = default(T)) where T : struct, IComponent;

        /// <summary>
        /// The method returns a component of a given type if it belongs to
        /// the specified entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a component of a given type if it belongs to
        /// the specified entity</returns>

        T GetComponent<T>(EntityId entityId) where T : struct, IComponent;
        
        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        void RemoveComponent<T>(EntityId entityId) where T : struct, IComponent;

        /// <summary>
        /// The method removes all components that are attached to the entity with the specified identifier
        /// <param name="entityId">Entity's identifier</param>
        /// </summary>

        void RemoveAllComponents(EntityId entityId);

        /// <summary>
        /// The method checks up whether a given entity has specified component or not
        /// </summary>
        /// <typeparam name="T">A type of a component</typeparam>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns true if the entity has the given component, false in other cases</returns>

        bool HasComponent<T>(EntityId entityId) where T : struct, IComponent;

        /// <summary>
        /// The method checks up whether a given entity has specified component or not
        /// </summary>
        /// <param name="componentType">A type of a component</param>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns true if the entity has the given component, false in other cases</returns>

        bool HasComponent(EntityId entityId, Type componentType);

        /// <summary>
        /// The method creates a new iterator which provides an ability to enumerate all components of a given entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a reference to IComponentIterator that implements some iterative mechanism</returns>

        IComponentIterator GetComponentsIterator(EntityId entityId);

        /// <summary>
        /// The method returns a reference to IEventManager implementation
        /// </summary>

        IEventManager EventManager { get; }

        /// <summary>
        /// The property returns a number of all active components that are used by entities
        /// </summary>

        uint NumOfActiveComponents { get; }

        /// <summary>
        /// The property returns an average number of components per entity. The value shows up
        /// an average entity's complexity, the higher the value, the worse performance 
        /// </summary>

        uint AverageNumOfComponentsPerEntity { get; }
    }
}
