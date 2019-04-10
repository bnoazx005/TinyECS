﻿namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface ISystemManager
    /// 
    /// The interface represents a functionality that every system manager in the project
    /// should provide
    /// </summary>

    public interface ISystemManager
    {
        /// <summary>
        /// The method register specialized system type which is IInitSystem. The systems of this type
        /// is executed only once at start of an application. Please DON'T use this method use Register
        /// method instead.
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        uint RegisterInitSystem(IInitSystem system);

        /// <summary>
        /// The method register specialized system type which is IUpdateSystem. The systems of this type
        /// is executed every frame when the initialization's step is passed. Please DON'T use this method use Register
        /// method instead.
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        uint RegisterUpdateSystem(IUpdateSystem system);

        /// <summary>
        /// The method excludes a system with the given systemId from the manager if it exists
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>

        void UnregisterSystem(uint systemId);

        /// <summary>
        /// The method activates a system with the given systemId if it registered within the manager
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>
        /// <returns>An identifier of a system within the manager</returns>

        uint ActivateSystem(uint systemId);

        /// <summary>
        /// The method deactivates a system with the given systemId if it registered within the manager
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>
        /// <returns>An identifier of a system within the manager</returns>

        uint DeactivateSystem(uint systemId);

        /// <summary>
        /// The method initializes all active systems that implements IInitSystem interface
        /// </summary>

        void Init();
        
        /// <summary>
        /// The method executes all active systems. The method should be invoked within a main loop of a game
        /// </summary>
        /// <param name="dt">The value in milliseconds which tells how much time elapsed from the previous frame</param>

        void Update(float dt);
    }
}