using System;
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

        public uint RegisterInitSystem(IInitSystem system)
        {
            return _registerSystem(system, mActiveInitSystems);
        }

        /// <summary>
        /// The method register specialized system type which is IUpdateSystem. The systems of this type
        /// is executed every frame when the initialization's step is passed. Please DON'T use this method use Register
        /// method instead.
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint RegisterUpdateSystem(IUpdateSystem system)
        {
            return _registerSystem(system, mActiveUpdateSystems);
        }

        /// <summary>
        /// The method registers a given reactive system within the manager. Please DON'T use this method use Register
        /// method instead.
        /// </summary>
        /// <param name="system">A reference to IReactiveSystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint RegisterReactiveSystem(IReactiveSystem system)
        {
            return _registerSystem(system, mActiveReactiveSystems);
        }

        /// <summary>
        /// The method excludes a system with the given systemId from the manager if it exists
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>

        public void UnregisterSystem(uint systemId)
        {
            ISystem system = _getSystemById(mActiveSystems, systemId);

            mActiveSystems[(int)systemId] = null;

            mFreeEntries.AddLast((int)systemId);
        }

        /// <summary>
        /// The method activates a system with the given systemId if it registered within the manager
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint ActivateSystem(uint systemId)
        {
            ISystem system = null;

            try
            {
                system = _getSystemById(mDeactivatedSystems, systemId);
            }
            catch (InvalidIdentifierException)
            {
                system = _getSystemById(mActiveSystems, systemId);

                /// do nothing with an already active system
                if (system != null)
                {
                    return systemId;
                }

                throw;
            }

            mDeactivatedSystems.RemoveAt((int)systemId);

            int registeredSystemId = 0;

            /// if there is free entries in the current array use it
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

        /// <summary>
        /// The method deactivates a system with the given systemId if it registered within the manager
        /// </summary>
        /// <param name="systemId">An identifier of a system which was retrieved from RegisterSystem's call</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint DeactivateSystem(uint systemId)
        {
            ISystem system = null;

            try
            {
                system = _getSystemById(mActiveSystems, systemId);
            }
            catch (InvalidIdentifierException)
            {
                system = _getSystemById(mDeactivatedSystems, systemId);

                /// do nothing with an already deactivated system
                if (system != null)
                {
                    return systemId;
                }

                throw;
            }

            mActiveSystems[(int)systemId] = null;

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

            foreach (var currUpdateSystem in mActiveUpdateSystems)
            {
                currUpdateSystem?.Update(dt);
            }
            
            mReactiveSystemsBuffer.Clear();
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

        public uint _registerSystem<T>(T system, List<T> specializedSystemsArray) 
            where T: class, ISystem
        {
            if (system == null)
            {
                throw new ArgumentNullException("system", "An input argument 'system' cannot equal to null");
            }

            int registeredSystemId = 0;

            // if the system's already registered just return its identifier
            if ((registeredSystemId = mActiveSystems.FindIndex(t => t == system)) >= 0)
            {
                return (uint)registeredSystemId;
            }
            else if ((registeredSystemId = mDeactivatedSystems.FindIndex(t => t == system)) >= 0)
            {
                return (uint)registeredSystemId;
            }

            /// if there is free entries in the current array use it
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

            int specializedSystemId = specializedSystemsArray.Count;

            specializedSystemsArray.Add(system);

            // NOTE: entity's identifier constists of two parts. The high 2 bytes equals to index within specialized array
            // and low 2 bytes are an index within common array
            return (uint)((specializedSystemId << 16) | registeredSystemId);
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
    }
}
