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
        protected IComponentManager   mComponentManager;

        protected IList<IEntity>      mEntitiesList;

        protected LinkedList<IEntity> mDestroyedEntitiesList;

        protected Queue<uint>         mNextFreeEntityId;

        protected uint                mEntitiesIdCounter;

        protected static string       mDefaultEntityPatternName = "Entity{0}";

        /// <summary>
        /// The main constructor of the class
        /// </summary>
        /// <param name="componentManager"></param>

        public EntityManager(IComponentManager componentManager)
        {
            mComponentManager = componentManager ?? throw new ArgumentNullException("componentManager");

            mEntitiesList = new List<IEntity>();

            mNextFreeEntityId = new Queue<uint>();

            mDestroyedEntitiesList = new LinkedList<IEntity>();

            mEntitiesIdCounter = 0;
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

            mEntitiesList.Add(newEntityInstance);

            return newEntityInstance;
        }

        /// <summary>
        /// The method destroy an entity with a given identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>The method returns true if the entity was successfully destroyed and false in other cases</returns>

        public bool DestroyEntity(uint entityId)
        {
            if (entityId >= mEntitiesList.Count)
            {
                return false;
            }
            
            mDestroyedEntitiesList.AddLast(mEntitiesList[(int)entityId]);

            mEntitiesList[(int)entityId] = null;

            mNextFreeEntityId.Enqueue(entityId);

            return true;
        }

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>
        /// <returns>A component's value</returns>

        public T AddComponent<T>(uint entityId) where T : struct, IComponent
        {
            return mComponentManager.AddComponent<T>(entityId);
        }

        /// <summary>
        /// The method replaces existing component's value 
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be updated</typeparam>

        public void ReplaceComponent<T>(uint entityId) where T : struct, IComponent
        {
            mComponentManager.ReplaceComponent<T>(entityId);
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
    }
}
