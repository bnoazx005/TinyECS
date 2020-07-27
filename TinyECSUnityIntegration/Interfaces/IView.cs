using TinyECS.Interfaces;


namespace TinyECSUnityIntegration.Interfaces
{
    /// <summary>
    /// interface IView
    /// 
    /// The interface describes a basic functionality of views that are used to represent current
    /// active state of a world's context using Unity3D engine. Basically, a view is a simple
    /// link between an entity and a GameObject's instance
    /// </summary>

    public interface IView
    {
        /// <summary>
        /// The method prepares the view for initialization step
        /// </summary>
        /// <param name="worldContext">A reference to IWorldContext implementation</param>

        void PreInit(IWorldContext worldContext);

        /// <summary>
        /// The method links current view's instance with a given entity's identifier
        /// </summary>
        /// <param name="entityId">An integral identifier which represents some existing entity</param>

        void Link(EntityId entityId);

        /// <summary>
        /// The method is used to provide a single place where all subscriptions are made
        /// </summary>
        /// <param name="eventManager">A reference to IEventManager implementation</param>
        /// <param name="entityId">Entity's identifier</param>

        void RegisterSubscriptions(IEventManager eventManager, EntityId entityId);

        /// <summary>
        /// The property returns a reference to IWorldContext which contains the entity that's linked
        /// to the view
        /// </summary>

        IWorldContext WorldContext { get; set; }

        /// <summary>
        /// The property returns an identifier of the linked entity
        /// </summary>

        EntityId LinkedEntityId { get; }
    }
}
