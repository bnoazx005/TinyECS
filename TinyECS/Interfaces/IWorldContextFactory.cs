namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface IWorldContextFactory
    /// 
    /// The interface describes a functionality of a world context's factory
    /// </summary>

    public interface IWorldContextFactory
    {
        /// <summary>
        /// The method creates a new instance of IWorldContext type
        /// </summary>
        /// <returns>The method returns a reference to a new instance of IWorldContext type</returns>

        IWorldContext CreateNewWorldInstance();
    }
}
