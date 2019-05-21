using System;
using System.Collections.Generic;


namespace TinyECS.Interfaces
{
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

        uint CreateEntity(string name = null);

        /// <summary>
        /// The method destroy an entity with a given identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>The method returns true if the entity was successfully destroyed and false in other cases</returns>

        bool DestroyEntity(uint entityId);

        /// <summary>
        /// The method returns a reference to an entity by its identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>A reference to IEntity's implementation</returns>

        IEntity GetEntityById(uint entityId);

        /// <summary>
        /// The method returns an array of entities that have all specified components attached to them
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have all specified components attached to them</returns>

        List<uint> GetEntitiesWithAll(params Type[] components);

        /// <summary>
        /// The method returns an array of entities that have any of specified components 
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have any of specified components</returns>

        List<uint> GetEntitiesWithAny(params Type[] components);

        /// <summary>
        /// The method returns a reference to IEventManager implementation
        /// </summary>

        IEventManager EventManager { get; }

        /// <summary>
        /// The property returns a number of active entities at the moment
        /// </summary>

        uint NumOfActiveEntities { get; }
    }
}
