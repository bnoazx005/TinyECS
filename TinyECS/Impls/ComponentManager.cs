using System;
using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// class ComponentManager
    /// 
    /// The class is an implementation of a component manager
    /// </summary>

    public class ComponentManager: IComponentManager
    {
        protected IDictionary<uint, IDictionary<Type, int>> mEntity2ComponentsHashTable;

        protected IDictionary<Type, int>                    mComponentTypesHashTable;

        protected IList<IList<IComponent>>                  mComponentsMatrix;

        public ComponentManager()
        {
            mEntity2ComponentsHashTable = new Dictionary<uint, IDictionary<Type, int>>();

            mComponentsMatrix = new List<IList<IComponent>>();
        }

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>
        /// <returns>A component's value</returns>

        public T AddComponent<T>(uint entityId) where T : struct, IComponent
        {
            // check does the entity have the corresponding component already
            if (!mEntity2ComponentsHashTable.ContainsKey(entityId))
            {
                // there is no an entity with corresponding id in the table
                mEntity2ComponentsHashTable.Add(entityId, new Dictionary<Type, int>());
            }

            var entityComponentsTable = mEntity2ComponentsHashTable[entityId];

            // create a new component

            return default(T);
        }

        /// <summary>
        /// The method returns a component of a given type if it belongs to
        /// the specified entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be retrieved</typeparam>
        /// <param name="entityId">Entity's identifier</param>
        /// <returns>The method returns a component of a given type if it belongs to
        /// the specified entity</returns>

        public T GetComponent<T>(uint entityId) where T : struct, IComponent
        {
            /// there is no entity with the specified id
            if (!mEntity2ComponentsHashTable.ContainsKey(entityId))
            {
                throw new EntityDoesntExistException(entityId);
            }

            var entityComponentsTable = mEntity2ComponentsHashTable[entityId];

            /// there is no component with specified type that belongs to the entity
            Type cachedComponentType = typeof(T);

            if (!entityComponentsTable.ContainsKey(cachedComponentType))
            {
                throw new ComponentDoesntExistException(cachedComponentType, entityId);
            }

            int componentTypeGroupHashValue = mComponentTypesHashTable[cachedComponentType];
            int componentHashValue          = entityComponentsTable[cachedComponentType];


            return (T)mComponentsMatrix[componentTypeGroupHashValue][componentHashValue];
        }

        /// <summary>
        /// The method replaces existing component's value 
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be updated</typeparam>

        public void ReplaceComponent<T>(uint entityId) where T : struct, IComponent
        {

        }

        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <param name="entityId">Entity's identifier</param>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        public void RemoveComponent<T>(uint entityId) where T : struct, IComponent
        {

        }
    }
}
