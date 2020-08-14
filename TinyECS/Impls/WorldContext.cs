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

        public EntityId CreateEntity(string name = null)
        {
            IEntity newEntity = mEntityManager.CreateEntity(name);

            return newEntity.Id;
        }

        /// <summary>
        /// The method destroy an entity with a given identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>The method returns true if the entity was successfully destroyed and false in other cases</returns>

        public bool DestroyEntity(EntityId entityId)
        {
            return mEntityManager.DestroyEntity(entityId);
        }

        /// <summary>
        /// The method returns a reference to an entity by its identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>A reference to IEntity's implementation</returns>

        public IEntity GetEntityById(EntityId entityId)
        {
            return mEntityManager.GetEntityById(entityId);
        }

        /// <summary>
        /// The method returns single entity which corresponds to given list of components
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>Returns a reference to an entity or null if it doesn't exist</returns>

        public IEntity GetSingleEntityWithAll(params Type[] components)
        {
            return mEntityManager.GetSingleEntityWithAll(components);
        }

        /// <summary>
        /// The method returns single entity which have any from given list of components
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>Returns a reference to an entity or null if it doesn't exist</returns>

        public IEntity GetSingleEntityWithAny(params Type[] components)
        {
            return mEntityManager.GetSingleEntityWithAny(components);
        }

        /// <summary>
        /// The method returns an array of entities that have all specified components attached to them
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have all specified components attached to them</returns>

        public List<EntityId> GetEntitiesWithAll(params Type[] components)
        {
            return mEntityManager.GetEntitiesWithAll(components);
        }

        /// <summary>
        /// The method returns an array of entities that have any of specified components 
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have any of specified components</returns>

        public List<EntityId> GetEntitiesWithAny(params Type[] components)
        {
            return mEntityManager.GetEntitiesWithAny(components);
        }

        /// <summary>
        /// The method returns an entity with specified component. Note that the component should be unique
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <returns> The method returns an entity with specified component. Note that the component should be unique </returns>

        public IEntity GetUniqueEntity<T>()
            where T : struct, IUniqueComponent
        {
            var entities = mEntityManager.GetEntitiesWithAny(typeof(T));

            if (entities == null || entities.Count < 1)
            {
                IEntity entity = mEntityManager.CreateEntity();
                entity.AddComponent<T>();

                return entity;
            }

            return mEntityManager.GetEntityById(entities[0]);
        }

        /// <summary>
        /// The method returns a reference to IEventManager implementation
        /// </summary>

        public IEventManager EventManager => mEntityManager?.EventManager;

        /// <summary>
        /// The property returns statistics of current world's context
        /// </summary>

        public TWorldContextStats Statistics => new TWorldContextStats
        {
            mNumOfActiveEntities             = mEntityManager?.NumOfActiveEntities             ?? 0,
            mNumOfReservedEntities           = mEntityManager?.NumOfReusableEntities           ?? 0,
            mNumOfActiveComponents           = mEntityManager?.NumOfActiveComponents           ?? 0,
            mAverageNumOfComponentsPerEntity = mEntityManager?.AverageNumOfComponentsPerEntity ?? 0
        };
    }
}
