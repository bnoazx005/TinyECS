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
            where T : struct
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
        /// <param name="destListenerId">An identifier of destination listener. If the value equals to uint.MaxValue
        /// the broadcasting will be executed</param>

        public void Notify<T>(T eventData, uint destListenerId = uint.MaxValue)
            where T : struct
        {
            Type currEventType = typeof(T);

            TListenerEntry currListenerEntry;

            if (destListenerId != uint.MaxValue)
            {
                if (destListenerId >= mListeners.Count)
                {
                    throw new ArgumentOutOfRangeException("destListenerId");
                }

                currListenerEntry = mListeners[(int)destListenerId];

                try
                {
                    if (currListenerEntry.mEventType == currEventType)
                    {
                        (currListenerEntry.mListener as IEventListener<T>)?.OnEvent(eventData);
                    }
                }
                finally
                {
                }
                

                return;
            }

            // execute broadcasting
            List<Exception> catchedExceptions = new List<Exception>();

            for (int i = 0; i < mListeners.Count; ++i)
            {
                currListenerEntry = mListeners[i];

                if (currListenerEntry.mEventType == null ||
                    currListenerEntry.mEventType != currEventType ||
                    currListenerEntry.mListener == null)
                {
                    continue;
                }

                try
                {
                    (currListenerEntry.mListener as IEventListener<T>)?.OnEvent(eventData);
                }
                catch(Exception exception)
                {
                    catchedExceptions.Add(exception);
                }
            }

            if (catchedExceptions.Count > 0)
            {
                throw new AggregateException(catchedExceptions);
            }
        }
    }
}
