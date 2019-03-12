using System;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// class Entity
    /// 
    /// The class represents an entity. An entity is one of atomic elements of
    /// entity-component-system paradigm. The class is used to simplify an access
    /// to components of an entity, but actually it's just an integer not a container
    /// </summary>

    public class Entity: IEntity
    {
        protected uint           mId;

        protected string         mName;

        protected IEntityManager mEntityManager;

        /// <summary>
        /// The main constructor with parameters
        /// </summary>
        /// <param name="entityManager">A reference to IEntityManager's implementation</param>
        /// <param name="id">An identifier of a entity (unique)</param>
        /// <param name="name">A name of an entity (two or more entities can have same name)</param>

        public Entity(IEntityManager entityManager, uint id, string name = null)
        {
            mEntityManager = entityManager ?? throw new ArgumentNullException("entityManager");

            mId = id;

            mName = name;
        }

        /// <summary>
        /// The default constructor is prohibited
        /// </summary>

        protected Entity()
        {
        }

        /// <summary>
        /// The method attaches a new component to the entity
        /// </summary>
        /// <typeparam name="T">A type of a component that should be attached</typeparam>
        /// <param name="componentInitializer">A type's value that is used to initialize fields of a new component</param>

        public void AddComponent<T>(T componentInitializer = default(T)) where T : struct, IComponent
        {
            mEntityManager.AddComponent<T>(mId, componentInitializer);
        }

        /// <summary>
        /// The method replaces existing component's value 
        /// </summary>
        /// <typeparam name="T">A type of a component that should be updated</typeparam>

        public void ReplaceComponent<T>() where T : struct, IComponent
        {
            mEntityManager.ReplaceComponent<T>(mId);
        }

        /// <summary>
        /// The method removes a component of a specified type
        /// </summary>
        /// <typeparam name="T">A type of a component that should be removed</typeparam>

        public void RemoveComponent<T>() where T : struct, IComponent
        {
            mEntityManager.RemoveComponent<T>(mId);
        }

        /// <summary>
        /// The property returns an identifier of an entity
        /// </summary>

        public uint Id => mId;

        /// <summary>
        /// The property returns a name of an entity
        /// </summary>

        public string Name => mName;
    }
}
