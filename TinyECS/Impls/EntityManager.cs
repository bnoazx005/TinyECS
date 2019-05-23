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
        protected delegate bool FilterPredicate(uint entityId, params Type[] componentTypes);

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

            entityId = mNextFreeEntityId.Count > 0 ? mNextFreeEntityId.Dequeue() : mEntitiesIdCounter++;

            IEntity newEntityInstance = null;

            if (mDestroyedEntitiesList.Count > 0)
            {
                /// reuse the previously created entity
                newEntityInstance = mDestroyedEntitiesList.First.Value;

                mDestroyedEntitiesList.RemoveFirst();
            }
            else
            {
                newEntityInstance = new Entity(this, entityId, name ?? string.Format(mDefaultEntityPatternName, entityId));
            }

            // register the entity within the components manager
            mComponentManager.RegisterEntity(entityId);

            mEntitiesList.Add(newEntityInstance);

            mEventManager.Notify<TNewEntityCreatedEvent>(new TNewEntityCreatedEvent()
            {
                mEntityId = entityId
            });

            ++mNumOfActiveEntities;

            return newEntityInstance;
        }

        /// <summary>
        /// The method destroy an entity with a given identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>The method returns true if the entity was successfully destroyed and false in other cases</returns>

        public bool DestroyEntity(uint entityId)
        {
            if (entityId >= mEntitiesList.Count || mEntitiesList[(int)entityId] == null)
            {
                return false;
            }
            
            mDestroyedEntitiesList.AddLast(mEntitiesList[(int)entityId]);

            mEntitiesList[(int)entityId] = null;

            mNextFreeEntityId.Enqueue(entityId);

            --mNumOfActiveEntities;

            return true;
        }

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>

        public void AddComponent<T>(uint entityId, T componentInitializer = default(T)) where T : struct, IComponent
        {
            mComponentManager.AddComponent<T>(entityId, componentInitializer);
        }
        
        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        public void RemoveComponent<T>(uint entityId) where T : struct, IComponent
        {
            mComponentManager.RemoveComponent<T>(entityId);
        }

        /// <summary>
        /// The method removes all components that are attached to the entity with the specified identifier
        /// <param name="entityId">Entity's identifier</param>
        /// </summary>

        public void RemoveAllComponents(uint entityId)
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

        public T GetComponent<T>(uint entityId) 
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

        public bool HasComponent<T>(uint entityId) 
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

        public bool HasComponent(uint entityId, Type componentType)
        {
            return mComponentManager.HasComponent(entityId, componentType);
        }

        /// <summary>
        /// The method returns a reference to an entity by its integral identifier
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a reference to an entity by its integral identifier</returns>

        public IEntity GetEntityById(uint entityId)
        {
            IEntity currEntity = null;

            if (entityId >= mEntitiesList.Count || (currEntity = mEntitiesList[(int)entityId]) ==null)
            {
                throw new EntityDoesntExistException(entityId);
            }

            return currEntity;
        }

        /// <summary>
        /// The method returns an array of entities that have all specified components attached to them
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have all specified components attached to them</returns>

        public List<uint> GetEntitiesWithAll(params Type[] components)
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
            }, components);
        }

        /// <summary>
        /// The method returns an array of entities that have any of specified components 
        /// </summary>
        /// <param name="components">A list of components that every entity should have</param>
        /// <returns>The method returns an array of entities that have any of specified components</returns>

        public List<uint> GetEntitiesWithAny(params Type[] components)
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
            }, components);
        }

        protected List<uint> _getFilteredEntities(FilterPredicate predicate, params Type[] components)
        {
            // TODO: should be cached somehow to decrease allocations count
            List<uint> filteredEntities = new List<uint>();

            for (uint currEntityId = 0; currEntityId < mEntitiesList.Count; ++currEntityId)
            {               
                // add an entity only if it passed all tests
                if (mEntitiesList[(int)currEntityId] != null && predicate(currEntityId, components))
                {
                    filteredEntities.Add(currEntityId);
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
    }
}
