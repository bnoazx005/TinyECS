using System;
using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    public class SystemsPackage: ISystemsPackage
    {
        private ISystemManager mSystemManager = null;

        private List<ISystem> mSystems = null;

        private List<uint> mSystemsIds = null;

        private bool mIsRegistered = false;

        public SystemsPackage(ISystemManager systemManager, List<ISystem> systems)
        {
            mSystemManager = systemManager ?? throw new ArgumentNullException("systemManager");
            mSystems = systems ?? throw new ArgumentNullException("systems");
        }

        public void Register()
        {
            if (mIsRegistered)
            {
                return;
            }

            mSystemsIds = new List<uint>();

            uint systemId = 0x0;

            for (int i = 0; i < mSystems.Count; ++i)
            {
                switch (mSystems[i])
                {
                    case IInitSystem initSystem:
                        systemId = mSystemManager.RegisterSystem(initSystem);
                        break;
                    case IUpdateSystem updateSystem:
                        systemId = mSystemManager.RegisterSystem(updateSystem);
                        break;
                    case IReactiveSystem reactiveSystem:
                        systemId = mSystemManager.RegisterSystem(reactiveSystem);
                        break;
                }
                
                mSystemsIds.Add(systemId);
            }

            mIsRegistered = true;
        }

        public void Unregister()
        {
            if (!mIsRegistered)
            {
                return;
            }

            for (int i = 0; i < mSystemsIds.Count; ++i)
            {
                mSystemManager.UnregisterSystem(mSystemsIds[i]);
            }

            mIsRegistered = false;
        }
    }
}
