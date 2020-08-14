﻿using System;
using System.Collections.Generic;


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

        bool DestroyEntity(EntityId entityId);

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>

        void AddComponent<T>(EntityId entityId, T componentInitializer = default(T)) where T : struct, IComponent;

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
        /// The method returns a component of a given type if it belongs to
        /// the specified entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a component of a given type if it belongs to
        /// the specified entity</returns>

        T GetComponent<T>(EntityId entityId) where T : struct, IComponent;

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
        /// The method returns a reference to an entity by its integral identifier
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a reference to an entity by its integral identifier</returns>

        IEntity GetEntityById(EntityId entityId);

        /// <summary>
        /// The method returns single entity which corresponds to given list of components
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>Returns a reference to an entity or null if it doesn't exist</returns>

        IEntity GetSingleEntityWithAll(params Type[] components);

        /// <summary>
        /// The method returns single entity which have any from given list of components
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>Returns a reference to an entity or null if it doesn't exist</returns>

        IEntity GetSingleEntityWithAny(params Type[] components);

        /// <summary>
        /// The method returns an array of entities that have all specified components attached to them
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have all specified components attached to them</returns>

        List<EntityId> GetEntitiesWithAll(params Type[] components);

        /// <summary>
        /// The method returns an array of entities that have any of specified components 
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have any of specified components</returns>

        List<EntityId> GetEntitiesWithAny(params Type[] components);

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
        /// The property returns a number of active entities at the moment
        /// </summary>

        uint NumOfActiveEntities { get; }

        /// <summary>
        /// The property returns a total number of reusable entities which are currently placed within
        /// entities pool, but can be retrieved for usage
        /// </summary>

        uint NumOfReusableEntities { get; }

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
