using TinyECS.Interfaces;

namespace TinyECS.Impls
{
    /// <summary>
    /// class WorldContextFactory
    /// 
    /// The class provides a default implementation of IWorldContext's factory which should be 
    /// used instead of manual context's creation
    /// </summary>

    public class WorldContextFactory: IWorldContextFactory
    {
        /// <summary>
        /// The method creates a new instance of IWorldContext type
        /// </summary>
        /// <returns>The method returns a reference to a new instance of IWorldContext type</returns>

        public IWorldContext CreateNewWorldInstance()
        {
            return new WorldContext(new EntityManager(new ComponentManager(new EventManager())));
        }
    }
}
