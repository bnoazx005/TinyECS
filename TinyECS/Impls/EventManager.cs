using System;
using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// class EventManager
    /// 
    /// The class is an implementation of internal events manager which is used to process
    /// reactive systems and provide a communications with Unity's GameObjects
    /// </summary>

    public class EventManager: IEventManager
    {
        protected struct TListenerEntry
        {
            public Type           mEventType;

            public IEventListener mListener;
        }

        protected List<TListenerEntry> mListeners;

        protected Stack<int>           mFreeEntriesRegistry;

        public EventManager()
        {
            mListeners = new List<TListenerEntry>();

            mFreeEntriesRegistry = new Stack<int>();
        }

        /// <summary>
        /// The method subscribes a listener to the manager
        /// </summary>
        /// <param name="eventListener">A reference to IEventListener implementation</param>
        /// <typeparam name="T">A type of an event</typeparam>
        /// <returns></returns>

        public uint Subscribe<T>(IEventListener eventListener)
            where T : struct, IEvent
        {
            if (eventListener == null)
            {
                throw new ArgumentNullException("eventListener");
            }
            
            int firstFreeEntryIndex = mFreeEntriesRegistry.Count > 0 ? mFreeEntriesRegistry.Pop() : mListeners.Count;

            if (firstFreeEntryIndex >= mListeners.Count)
            {
                mListeners.Add(new TListenerEntry { });
            }

            mListeners[firstFreeEntryIndex] = new TListenerEntry()
            {
                mEventType = typeof(T),
                mListener  = eventListener
            };

            return (uint)firstFreeEntryIndex;
        }

        /// <summary>
        /// The method unsubscribes specified listener with a given identifier
        /// </summary>
        /// <param name="listenerId">An identifier of a listener</param>

        public void Unsubscribe(uint listenerId)
        {
            if (listenerId >= mListeners.Count)
            {
                throw new ListenerDoesntExistException(listenerId);
            }

            mListeners[(int)listenerId] = new TListenerEntry() { };

            mFreeEntriesRegistry.Push((int)listenerId);
        }

        /// <summary>
        /// The method notifies all listeners of the manager that an event of type T has occurred
        /// </summary>
        /// <typeparam name="T">A type of an event</typeparam>
        /// <param name="eventData">An event's data</param>

        public void Notify<T>(T eventData)
            where T : struct, IEvent
        {
            Type currEventType = typeof(T);

            foreach (TListenerEntry currListenerEntry in mListeners)
            {
                if (currListenerEntry.mEventType == null ||
                    currListenerEntry.mEventType != currEventType ||
                    currListenerEntry.mListener == null)
                {
                    continue;
                }

                (currListenerEntry.mListener as IEventListener<T>).OnEvent(eventData);
            }
        }
    }
}
