namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface ISystemsGroup
    /// 
    /// The interface describes a functionality of a group of systems that can be unified by some criteria
    /// </summary>

    public interface ISystemsGroup
    {
        /// <summary>
        /// The method adds a new system into the group
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>

        void Add(ISystem system);
    }
}
