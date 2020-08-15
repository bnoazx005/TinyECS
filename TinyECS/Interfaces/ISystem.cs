using System;
using System.Collections.Generic;


namespace TinyECS.Interfaces
{
    public struct SystemId
    {
        public static SystemId Invalid = new SystemId(uint.MaxValue);

        private uint mInternalID;

        public SystemId(uint value)
        {
            mInternalID = value;
        }

        public static explicit operator SystemId(uint id) => new SystemId(id);
        public static explicit operator SystemId(int id)  => new SystemId((uint)id);
        public static explicit operator uint(SystemId id) => id.mInternalID;
        public static explicit operator int(SystemId id)  => (int)id.mInternalID;

        public static bool operator ==(SystemId left, SystemId right) => left.mInternalID == right.mInternalID; 
        public static bool operator !=(SystemId left, SystemId right) => left.mInternalID != right.mInternalID; 
        
        public override bool Equals(object obj) => mInternalID == ((SystemId)obj).mInternalID;
        public override int GetHashCode() => mInternalID.GetHashCode();
        public override string ToString() => $"SystemId: {mInternalID}";
    }


    [Flags]
    public enum E_SYSTEM_TYPE: byte
    {
        ST_UNKNOWN  = 0x0,
        ST_INIT     = 0x1,
        ST_UPDATE   = 0x2,
        ST_REACTIVE = 0x4
    }


    /// <summary>
    /// interface ISystem
    /// 
    /// The interface describes the most common functionality that all
    /// systems should implement
    /// </summary>

    public interface ISystem
    {
    }


    /// <summary>
    /// interface IInitSystem
    /// 
    /// The interface represents a type of systems that are used only on initialization step of an application
    /// </summary>

    public interface IInitSystem: ISystem
    {
        /// <summary>
        /// The method is called once on initialization step
        /// </summary>

        void Init();
    }


    /// <summary>
    /// interface IUpdateSystem
    /// 
    /// The interface represents a type of systems that should be updated every frame
    /// </summary>

    public interface IUpdateSystem: ISystem
    {
        /// <summary>
        /// The method is called every frame
        /// </summary>
        /// <param name="deltaTime">A time which is elapsed from the previous frame</param>

        void Update(float deltaTime);
    }


    /// <summary>
    /// interface IReactiveSystem
    /// 
    /// The interface describes a functionality of a systems that are updated only specific
    /// set of entities that were changed and passed internal filters
    /// </summary>

    public interface IReactiveSystem: ISystem
    {
        /// <summary>
        /// The method filters input list of entities based on its internal predicate's implementation
        /// </summary>
        /// <param name="entity">An input entity</param>
        /// <returns>The method should return true to pass the entity, false in other cases</returns>

        bool Filter(IEntity entity);

        /// <summary>
        /// The method is called if some entity was changed and passed a filter of the system
        /// </summary>
        /// <param name="entities">A list of entities that are passed a filter of the system</param>
        /// <param name="deltaTime">A time which is elapsed from the previous frame</param>

        void Update(List<IEntity> entities, float deltaTime);
    }


    /// <summary>
    /// class ISystemIterator
    /// 
    /// The interface describes a functionality of an iterator that enumerates all systems
    /// </summary>

    public interface ISystemIterator
    {
        /// <summary>
        /// The method returns system's value which the iterator points to
        /// </summary>
        /// <typeparam name="T">A specific type to which current system will be casted</typeparam>
        /// <returns>The method returns system's value which the iterator points to</returns>

        T Get<T>() where T : struct, ISystem;

        /// <summary>
        /// The method returns a reference to ISystem which the iterator points to
        /// </summary>
        /// <returns>The method returns a reference to ISystem which the iterator points to</returns>

        ISystem Get();

        /// <summary>
        /// The method moves iterator to next available system if the latter exists
        /// </summary>
        /// <returns>The method returns true if there is a system at next position, false in other cases</returns>

        bool MoveNext();
    }


    /// <summary>
    /// interface ISystemsPackage
    /// 
    /// The interface describes a functionality of a package that allows to register, execute all systems as a single 
    /// unit
    /// </summary>

    public interface ISystemsPackage
    {
        void Register();
        void Unregister();
    }
}
