namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface IEventListener
    /// 
    /// The interface represents a listener of events of specific type
    /// </summary>

    public interface IEventListener
    {
    }


    /// <summary>
    /// interface IEventListener
    /// 
    /// The interface represents a listener of events of specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public interface IEventListener<T>: IEventListener
        where T: struct, IEvent
    {
        /// <summary>
        /// The method should be implemented by all listeners which want to retrieve events of T type
        /// </summary>
        /// <param name="eventData">An event's data</param>

        void OnEvent(T eventData);
    }


    /// <summary>
    /// interface IEventManager
    /// 
    /// The interface describes a functionality of an internal event manager which is used by the framework
    /// </summary>

    public interface IEventManager
    {
        /// <summary>
        /// The method subscribes a listener to the manager
        /// </summary>
        /// <param name="eventListener">A reference to IEventListener implementation</param>
        /// <typeparam name="T">A type of an event</typeparam>
        /// <returns>An identifier of a listener</returns>

        uint Subscribe<T>(IEventListener eventListener)
            where T : struct, IEvent;

        /// <summary>
        /// The method unsubscribes specified listener with a given identifier
        /// </summary>
        /// <param name="listenerId">An identifier of a listener</param>

        void Unsubscribe(uint listenerId);

        /// <summary>
        /// The method notifies all listeners of the manager that an event of type T has occurred
        /// </summary>
        /// <typeparam name="T">A type of an event</typeparam>
        /// <param name="eventData">An event's data</param>
        /// <param name="destListenerId">An identifier of destination listener. If the value equals to uint.MaxValue
        /// the broadcasting will be executed</param>

        void Notify<T>(T eventData, uint destListenerId = uint.MaxValue)
            where T: struct, IEvent;
    }
}
