using System;
using System.Collections;
using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// class SystemManager
    /// 
    /// The class is a basic implementation of ISystemManager interface
    /// </summary>

    public class SystemManager: ISystemManager, IEventListener<TNewComponentAddedEvent>
    {
        public class SystemIterator: ISystemIterator
        {
            protected IEnumerator<ISystem> mSystemInternalIterator;

            public SystemIterator(IEnumerator<ISystem> systemsInternalIter)
            {
                mSystemInternalIterator = systemsInternalIter;
            }

            /// <summary>
            /// The method returns system's value which the iterator points to
            /// </summary>
            /// <typeparam name="T">A specific type to which current system will be casted</typeparam>
            /// <returns>The method returns system's value which the iterator points to</returns>

            public T Get<T>() where T : struct, ISystem
            {
                return (T)Get();
            }

            /// <summary>
            /// The method returns a reference to ISystem which the iterator points to
            /// </summary>
            /// <returns>The method returns a reference to ISystem which the iterator points to</returns>

            public ISystem Get()
            {
                return mSystemInternalIterator.Current;
            }

            /// <summary>
            /// The method moves iterator to next available system if the latter exists
            /// </summary>
            /// <returns>The method returns true if there is a system at next position, false in other cases</returns>

            public bool MoveNext()
            {
                return mSystemInternalIterator.MoveNext();
            }
        }

        protected IWorldContext         mWorldContext;

        protected IEventManager         mEventManager;

        protected List<ISystem>         mActiveSystems;

        protected List<IInitSystem>     mActiveInitSystems;

        protected List<IUpdateSystem>   mActiveUpdateSystems;

        protected List<IReactiveSystem> mActiveReactiveSystems;

        protected List<ISystem>         mDeactivatedSystems;

        protected LinkedList<int>       mFreeEntries;

        protected List<IEntity>         mReactiveSystemsBuffer;

        public SystemManager(IWorldContext worldContext)
        {
            mWorldContext = worldContext ?? throw new ArgumentNullException("worldContext");

            mEventManager = worldContext?.EventManager;

            // listen to events of the world context to provide reactive behaviour of systems
            mEventManager.Subscribe<TNewComponentAddedEvent>(this);

            mActiveSystems         = new List<ISystem>();
            mActiveInitSystems     = new List<IInitSystem>();
            mActiveUpdateSystems   = new List<IUpdateSystem>();
            mActiveReactiveSystems = new List<IReactiveSystem>();
            mDeactivatedSystems    = new List<ISystem>();

            mFreeEntries = new LinkedList<int>();

            mReactiveSystemsBuffer = new List<IEntity>();
        }

        /// <summary>
        /// The method register specialized system type which is IInitSystem. The systems of this type
        /// is executed only once at start of an application. Please DON'T use this method use Register
        /// method instead.
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint RegisterSystem(IInitSystem system)
        {
            return _registerSystem(system, mActiveInitSystems, (byte)E_SYSTEM_TYPE.ST_INIT);
        }

        /// <summary>
        /// The method register specialized system type which is IUpdateSystem. The systems of this type
        /// is executed every frame when the initialization's step is passed. Please DON'T use this method use Register
        /// method instead.
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint RegisterSystem(IUpdateSystem system)
        {
            return _registerSystem(system, mActiveUpdateSystems, (byte)E_SYSTEM_TYPE.ST_UPDATE);
        }

        /// <summary>
        /// The method registers a given reactive system within the manager. Please DON'T use this method use Register
        /// method instead.
        /// </summary>
        /// <param name="system">A reference to IReactiveSystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint RegisterSystem(IReactiveSystem system)
        {
            return _registerSystem(system, mActiveReactiveSystems, (byte)E_SYSTEM_TYPE.ST_REACTIVE);
        }

        /// <summary>
        /// The method excludes a system with the given systemId from the manager if it exists
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>

        public void UnregisterSystem(uint systemId)
        {
            uint commonSystemId = systemId & 0xFFFF; /* extract 2 low bytes*/

            ISystem system = _getSystemById(mActiveSystems, commonSystemId);

            mActiveSystems[(int)commonSystemId] = null;

            mFreeEntries.AddLast((int)commonSystemId);
        }

        /// <summary>
        /// The method activates a system with the given systemId if it registered within the manager
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint ActivateSystem(uint systemId)
        {
            ISystem system = null;

            uint commonSystemId = systemId & 0xFFFF; /* extract 2 low bytes*/

            try
            {
                system = _getSystemById(mDeactivatedSystems, commonSystemId);
            }
            catch (InvalidIdentifierException)
            {
                system = _getSystemById(mActiveSystems, commonSystemId);

                /// do nothing with an already active system
                if (system != null)
                {
                    return systemId;
                }

                throw;
            }

            mDeactivatedSystems.RemoveAt((int)commonSystemId);

            return _pushSystemToActiveSystems(system);
        }

        /// <summary>
        /// The method deactivates a system with the given systemId if it registered within the manager
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint DeactivateSystem(uint systemId)
        {
            ISystem system = null;

            uint commonSystemId = systemId & 0xFFFF;

            try
            {
                system = _getSystemById(mActiveSystems, commonSystemId);
            }
            catch (InvalidIdentifierException)
            {
                system = _getSystemById(mDeactivatedSystems, commonSystemId);

                /// do nothing with an already deactivated system
                if (system != null)
                {
                    return systemId;
                }

                throw;
            }

            mActiveSystems[(int)commonSystemId] = null;

            int registeredSystemId = mDeactivatedSystems.Count;

            mDeactivatedSystems.Add(system);

            return (uint)registeredSystemId;
        }

        /// <summary>
        /// The method initializes all active systems that implements IInitSystem interface
        /// </summary>

        public void Init()
        {
            foreach (var currInitSystem in mActiveInitSystems)
            {
                currInitSystem?.Init();
            }
        }

        /// <summary>
        /// The method executes all active systems. The method should be invoked within a main loop of a game
        /// </summary>
        /// <param name="dt">The value in milliseconds which tells how much time elapsed from the previous frame</param>

        public void Update(float dt)
        {
            List<IEntity> filteredEntities = null;

            foreach (var currUpdateSystem in mActiveUpdateSystems)
            {
                currUpdateSystem?.Update(dt);
            }

            // TODO: execute all reactive systems
            foreach (var currReactiveSystem in mActiveReactiveSystems)
            {
                filteredEntities = mReactiveSystemsBuffer.FindAll(currReactiveSystem.Filter);

                if (filteredEntities.Count < 1)
                {
                    continue;
                }

                currReactiveSystem?.Update(filteredEntities, dt);
            }


            mReactiveSystemsBuffer.Clear();
        }

        /// <summary>
        /// The method creates a new iterator which provides an ability to enumerate all systems of a given manager
        /// </summary>
        /// <returns>The method returns a reference to ISystemIterator that implements some iterative mechanism</returns>

        public ISystemIterator GetSystemIterator()
        {
            return new SystemIterator(mActiveSystems.GetEnumerator());
        }

        protected ISystem _getSystemById(IList<ISystem> sourceArray, uint id)
        {
            ISystem system = null;

            if ((sourceArray.Count <= id) || ((system = sourceArray[(int)id]) == null))
            {
                throw new InvalidIdentifierException(id);
            }

            return system;
        }

        public uint _registerSystem<T>(T system, List<T> specializedSystemsArray, byte systemTypeMask = 0x0) 
            where T: class, ISystem
        {
            if (system == null)
            {
                throw new ArgumentNullException("system", "An input argument 'system' cannot equal to null");
            }

            int registeredSystemId  = 0;
            int specializedSystemId = 0;

            // if the system's already registered just return its identifier
            if ((registeredSystemId = mActiveSystems.FindIndex(t => t == system)) >= 0 && (specializedSystemId = specializedSystemsArray.FindIndex(t => t == system)) != -1)
            {
                return (uint)((specializedSystemId << 16) | registeredSystemId | (systemTypeMask << 29));
            }
            //else if ((registeredSystemId = mDeactivatedSystems.FindIndex(t => t == system)) >= 0)
            //{
            //    return (uint)registeredSystemId;
            //}

            registeredSystemId = (int)_pushSystemToActiveSystems(system);

            specializedSystemId = specializedSystemsArray.Count;

            specializedSystemsArray.Add(system);

            // NOTE: entity's identifier constists of two parts. The high 2 bytes equals to index within specialized array
            // and low 2 bytes are an index within common array
            return (uint)((specializedSystemId << 16) | registeredSystemId | (systemTypeMask << 29));
        }

        public void OnEvent(TNewComponentAddedEvent eventData)
        {
            _addEntityToReactiveSystemsBuffer(eventData.mOwnerId);
        }

        protected void _addEntityToReactiveSystemsBuffer(uint entityId)
        {
            IEntity entity = mWorldContext.GetEntityById(entityId);

            if (entity == null)
            {
                return;
            }

            //TODO: Maybe we should check up for duplicates of the entity later

            mReactiveSystemsBuffer.Add(entity);
        }

        protected uint _pushSystemToActiveSystems(ISystem system)
        {
            int registeredSystemId = mActiveSystems.FindIndex(t => t == system);

            if (registeredSystemId != -1)
            {
                return (uint)registeredSystemId;
            }

            /// if there are free entries in the current array use it
            if (mFreeEntries.Count >= 1)
            {
                registeredSystemId = mFreeEntries.First.Value;

                mFreeEntries.RemoveFirst();

                mActiveSystems[registeredSystemId] = system;
            }
            else
            {
                registeredSystemId = mActiveSystems.Count;

                mActiveSystems.Add(system);
            }

            return (uint)registeredSystemId;
        }

        protected E_SYSTEM_TYPE _getSystemTypeMask(ISystem system)
        {
            E_SYSTEM_TYPE systemTypeMask = E_SYSTEM_TYPE.ST_UNKNOWN;

            if (system is IInitSystem)
            {
                systemTypeMask |= E_SYSTEM_TYPE.ST_INIT;
            }

            if (system is IUpdateSystem)
            {
                systemTypeMask |= E_SYSTEM_TYPE.ST_UPDATE;
            }

            if (system is IReactiveSystem)
            {
                systemTypeMask |= E_SYSTEM_TYPE.ST_REACTIVE;
            }

            return systemTypeMask;
        }
    }
}
