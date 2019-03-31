namespace TinyECS.Interfaces
{
    /// <summary>
    /// class IWorldContext
    /// 
    /// The interface describes a functionality of a world's context which is a main hub
    /// that combines all parts of the architecture
    /// </summary>

    public interface IWorldContext
    {
        /// <summary>
        /// The method creates a new entity within the context
        /// </summary>
        /// <param name="name">An optional parameter that specifies a name of the entity</param>
        /// <returns>An integer index that is an entity's identifier</returns>

        uint CreateEntity(string name = null);

        /// <summary>
        /// The method returns a reference to an entity by its identifier
        /// </summary>
        /// <param name="entityId">An entity's identifier</param>
        /// <returns>A reference to IEntity's implementation</returns>

        IEntity GetEntityById(uint entityId);
    }
}
