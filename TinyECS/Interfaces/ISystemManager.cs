namespace TinyECS.Interfaces
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
        /// The method registers the given system within the manager
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        uint RegisterSystem(ISystem system);

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
    }
}