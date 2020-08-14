﻿using System;
using System.Collections.Generic;


namespace TinyECS.Interfaces
{
    /// <summary>
    /// structure TWorldContextStats
    /// 
    /// The structure contains statistics about a particular world's context
    /// </summary>

    public struct TWorldContextStats
    {
        public uint mNumOfActiveEntities;

        public uint mNumOfReservedEntities;

        public uint mNumOfActiveComponents;

        public uint mAverageNumOfComponentsPerEntity;
    }


    /// <summary>
    /// class IWorldContext
    /// 
    /// The interface describes a functionality of a world's context which is a main hub
    /// that combines all parts of the architecture
    /// </summary>

    public interface IWorldContext
    {
        /// <summary>
        /// The method creates a new entity within the context
        /// </summary>
        /// <param name="name">An optional parameter that specifies a name of the entity</param>
        /// <returns>An integer index that is an entity's identifier</returns>

        EntityId CreateEntity(string name = null);

        /// <summary>
        /// The method destroy an entity with a given identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>The method returns true if the entity was successfully destroyed and false in other cases</returns>

        bool DestroyEntity(EntityId entityId);

        /// <summary>
        /// The method returns a reference to an entity by its identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>A reference to IEntity's implementation</returns>

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
        /// The method returns an entity with specified component. Note that the component should be unique
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <returns> The method returns an entity with specified component. Note that the component should be unique </returns>

        IEntity GetUniqueEntity<T>() where T: struct, IUniqueComponent;

        /// <summary>
        /// The method returns a reference to IEventManager implementation
        /// </summary>

        IEventManager EventManager { get; }

        /// <summary>
        /// The property returns statistics of current world's context
        /// </summary>

        TWorldContextStats Statistics { get; }
    }
}
