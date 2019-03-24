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

    public class SystemManager: ISystemManager
    {
        protected List<ISystem>   mActiveSystems;

        protected List<ISystem>   mDeactivatedSystems;

        protected LinkedList<int> mFreeEntries;

        public SystemManager()
        {
            mActiveSystems      = new List<ISystem>();
            mDeactivatedSystems = new List<ISystem>();

            mFreeEntries = new LinkedList<int>();
        }

        /// <summary>
        /// The method registers the given system within the manager
        /// </summary>
        /// <param name="system">A reference to ISystem implementation</param>
        /// <returns>An identifier of a system within the manager</returns>

        public uint RegisterSystem(ISystem system)
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

            return (uint)registeredSystemId;
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

        protected ISystem _getSystemById(IList<ISystem> sourceArray, uint id)
        {
            ISystem system = null;

            if ((sourceArray.Count <= id) || ((system = sourceArray[(int)id]) == null))
            {
                throw new InvalidIdentifierException(id);
            }

            return system;
        }
    }
}
