using System;
using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    public class WorldContext: IWorldContext
    {
        protected IEntityManager mEntityManager;

        public WorldContext(IEntityManager entityManager)
        {
            mEntityManager = entityManager ?? throw new ArgumentNullException("entityManager");
        }

        /// <summary>
        /// The method creates a new entity within the context
        /// </summary>
        /// <param name="name">An optional parameter that specifies a name of the entity</param>
        /// <returns>An integer index that is an entity's identifier</returns>

        public uint CreateEntity(string name = null)
        {
            IEntity newEntity = mEntityManager.CreateEntity(name);

            return newEntity.Id;
        }

        /// <summary>
        /// The method returns a reference to an entity by its identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>A reference to IEntity's implementation</returns>

        public IEntity GetEntityById(uint entityId)
        {
            return mEntityManager.GetEntityById(entityId);
        }

        /// <summary>
        /// The method returns an array of entities that have all specified components attached to them
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have all specified components attached to them</returns>

        public List<uint> GetEntitiesWithAll(params Type[] components)
        {
            return mEntityManager.GetEntitiesWithAll(components);
        }

        /// <summary>
        /// The method returns an array of entities that have any of specified components 
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have any of specified components</returns>

        public List<uint> GetEntitiesWithAny(params Type[] components)
        {
            return mEntityManager.GetEntitiesWithAny(components);
        }
    }
}
