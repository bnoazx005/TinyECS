using System;
using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// class EntityManager
    /// 
    /// The class is a main infrastructure's element which manages with all entities
    /// in the library
    /// </summary>

    public class EntityManager: IEntityManager
    {
        protected delegate bool FilterPredicate(EntityId entityId, params Type[] componentTypes);

        protected IComponentManager   mComponentManager;

        protected IEventManager       mEventManager;

        protected IList<IEntity>      mEntitiesList;

        protected LinkedList<IEntity> mDestroyedEntitiesList;

        protected Queue<uint>         mNextFreeEntityId;

        protected uint                mEntitiesIdCounter;

        protected static string       mDefaultEntityPatternName = "Entity{0}";

        protected uint                mNumOfActiveEntities;

        /// <summary>
        /// The main constructor of the class
        /// </summary>
        /// <param name="componentManager"></param>

        public EntityManager(IComponentManager componentManager)
        {
            mComponentManager = componentManager ?? throw new ArgumentNullException("componentManager");

            mEventManager = mComponentManager?.EventManager;

            mEntitiesList = new List<IEntity>();

            mNextFreeEntityId = new Queue<uint>();

            mDestroyedEntitiesList = new LinkedList<IEntity>();

            mEntitiesIdCounter = 0;

            mNumOfActiveEntities = 0;
        }

        protected EntityManager()
        {
        }

        /// <summary>
        /// The method creates a new entity with a given name if it was specified
        /// </summary>
        /// <param name="name">An optional parameter that specifies a name of an entity</param>
        /// <returns>A reference to an entity</returns>

        public IEntity CreateEntity(string name = null)
        {
            uint entityId = 0;

            bool isEntityReused = mNextFreeEntityId.Count > 0;

            entityId = isEntityReused ? mNextFreeEntityId.Dequeue() : mEntitiesIdCounter++;

            IEntity newEntityInstance = null;

            if (mDestroyedEntitiesList.Count > 0)
            {
                /// reuse the previously created entity
                newEntityInstance = mDestroyedEntitiesList.First.Value;

                mDestroyedEntitiesList.RemoveFirst();
            }
            else
            {
                newEntityInstance = new Entity(this, (EntityId)entityId, name ?? string.Format(mDefaultEntityPatternName, entityId));
            }

            EntityId id = (EntityId)entityId;

            // register the entity within the components manager
            mComponentManager.RegisterEntity(id);

            if (isEntityReused)
            {
                mEntitiesList[(int)entityId] = newEntityInstance;
            }
            else
            {
                mEntitiesList.Add(newEntityInstance);
            }

            mEventManager.Notify<TNewEntityCreatedEvent>(new TNewEntityCreatedEvent()
            {
                mEntityId = id
            });

            ++mNumOfActiveEntities;

            return newEntityInstance;
        }

        /// <summary>
        /// The method destroy an entity with a given identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>The method returns true if the entity was successfully destroyed and false in other cases</returns>

        public bool DestroyEntity(EntityId entityId)
        {
            uint id = (uint)entityId;

            if (entityId == EntityId.Invalid || id >= mEntitiesList.Count || mEntitiesList[(int)id] == null)
            {
                return false;
            }

            IEntity destroyedEntity = mEntitiesList[(int)id];

            RemoveAllComponents(entityId);

            mDestroyedEntitiesList.AddLast(destroyedEntity);

            mEntitiesList[(int)id] = null;

            mNextFreeEntityId.Enqueue(id);

            mEventManager.Notify(new TEntityDestroyedEvent()
            {
                mEntityId   = entityId,
                mEntityName = destroyedEntity.Name
            });

            --mNumOfActiveEntities;

            return true;
        }

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>

        public void AddComponent<T>(EntityId entityId, T componentInitializer = default(T)) where T : struct, IComponent
        {
            mComponentManager.AddComponent<T>(entityId, componentInitializer);
        }
        
        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        public void RemoveComponent<T>(EntityId entityId) where T : struct, IComponent
        {
            mComponentManager.RemoveComponent<T>(entityId);
        }

        /// <summary>
        /// The method removes all components that are attached to the entity with the specified identifier
        /// <param name="entityId">Entity's identifier</param>
        /// </summary>

        public void RemoveAllComponents(EntityId entityId)
        {
            mComponentManager.RemoveAllComponents(entityId);
        }

        /// <summary>
        /// The method returns a component of a given type if it belongs to
        /// the specified entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a component of a given type if it belongs to
        /// the specified entity</returns>

        public T GetComponent<T>(EntityId entityId) 
            where T : struct, IComponent
        {
            return mComponentManager.GetComponent<T>(entityId);
        }

        /// <summary>
        /// The method checks up whether a given entity has specified component or not
        /// </summary>
        /// <typeparam name="T">A type of a component</typeparam>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns true if the entity has the given component, false in other cases</returns>

        public bool HasComponent<T>(EntityId entityId) 
            where T : struct, IComponent
        {
            return mComponentManager.HasComponent<T>(entityId);
        }

        /// <summary>
        /// The method checks up whether a given entity has specified component or not
        /// </summary>
        /// <param name="componentType">A type of a component</param>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns true if the entity has the given component, false in other cases</returns>

        public bool HasComponent(EntityId entityId, Type componentType)
        {
            return mComponentManager.HasComponent(entityId, componentType);
        }

        /// <summary>
        /// The method returns a reference to an entity by its integral identifier
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a reference to an entity by its integral identifier</returns>

        public IEntity GetEntityById(EntityId entityId)
        {
            IEntity currEntity = null;

            uint id = (uint)entityId;

            if (id >= mEntitiesList.Count || (currEntity = mEntitiesList[(int)id]) == null)
            {
                throw new EntityDoesntExistException(entityId);
            }

            return currEntity;
        }

        /// <summary>
        /// The method returns single entity which corresponds to given list of components
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>Returns a reference to an entity or null if it doesn't exist</returns>

        public IEntity GetSingleEntityWithAll(params Type[] components)
        {
            var entities = _getFilteredEntities((entityId, componentsTypes) =>
            {
                foreach (var currComponentType in componentsTypes)
                {
                    if (!mComponentManager.HasComponent(entityId, currComponentType))
                    {
                        return false;
                    }
                }

                return true;
            }, true, components);

            if (entities.Count < 1)
            {
                return null;
            }

            return GetEntityById(entities[0]);
        }

        /// <summary>
        /// The method returns single entity which have any from given list of components
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>Returns a reference to an entity or null if it doesn't exist</returns>

        public IEntity GetSingleEntityWithAny(params Type[] components)
        {
            var entities = _getFilteredEntities((entityId, componentsTypes) =>
            {
                foreach (var currComponentType in componentsTypes)
                {
                    if (mComponentManager.HasComponent(entityId, currComponentType))
                    {
                        return true;
                    }
                }

                return false;
            }, true, components);

            if (entities.Count < 1)
            {
                return null;
            }

            return GetEntityById(entities[0]);
        }

        /// <summary>
        /// The method returns an array of entities that have all specified components attached to them
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have all specified components attached to them</returns>

        public List<EntityId> GetEntitiesWithAll(params Type[] components)
        {
            return _getFilteredEntities((entityId, componentsTypes) =>
            {
                foreach (var currComponentType in componentsTypes)
                {
                    if (!mComponentManager.HasComponent(entityId, currComponentType))
                    {
                        return false;
                    }
                }

                return true;
            }, false, components);
        }

        /// <summary>
        /// The method returns an array of entities that have any of specified components 
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have any of specified components</returns>

        public List<EntityId> GetEntitiesWithAny(params Type[] components)
        {
            return _getFilteredEntities((entityId, componentsTypes) =>
            {
                foreach (var currComponentType in componentsTypes)
                {
                    if (mComponentManager.HasComponent(entityId, currComponentType))
                    {
                        return true;
                    }
                }

                return false;
            }, false, components);
        }

        /// <summary>
        /// The method creates a new iterator which provides an ability to enumerate all components of a given entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a reference to IComponentIterator that implements some iterative mechanism</returns>

        public IComponentIterator GetComponentsIterator(EntityId entityId)
        {
            return mComponentManager.GetComponentsIterator(entityId);
        }

        protected List<EntityId> _getFilteredEntities(FilterPredicate predicate, bool singleOnly, params Type[] components)
        {
            // TODO: should be cached somehow to decrease allocations count
            List<EntityId> filteredEntities = new List<EntityId>();

            for (uint currEntityId = 0; currEntityId < mEntitiesList.Count; ++currEntityId)
            {
                EntityId id = (EntityId)currEntityId;

                // add an entity only if it passed all tests
                if (mEntitiesList[(int)currEntityId] != null && predicate(id, components))
                {
                    filteredEntities.Add(id);

                    // If we need only the first element we could interrupt the loop
                    if (singleOnly)
                    {
                        break;
                    }
                }
            }

            return filteredEntities;
        }

        /// <summary>
        /// The method returns a reference to IEventManager implementation
        /// </summary>

        public IEventManager EventManager => mEventManager;

        /// <summary>
        /// The property returns a number of active entities at the moment
        /// </summary>

        public uint NumOfActiveEntities => mNumOfActiveEntities;

        /// <summary>
        /// The property returns a total number of reusable entities which are currently placed within
        /// entities pool, but can be retrieved for usage
        /// </summary>

        public uint NumOfReusableEntities => (uint)mDestroyedEntitiesList.Count;

        /// <summary>
        /// The property returns a number of all active components that are used by entities
        /// </summary>

        public uint NumOfActiveComponents => mComponentManager?.NumOfActiveComponents ?? 0;

        /// <summary>
        /// The property returns an average number of components per entity. The value shows up
        /// an average entity's complexity, the higher the value, the worse performance 
        /// </summary>

        public uint AverageNumOfComponentsPerEntity => mComponentManager?.AverageNumOfComponentsPerEntity ?? 0;
    }
}
