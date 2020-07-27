using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace TinyECSTests
{
    [TestFixture]
    public class SystemManagerTests
    {
        protected ISystemManager mSystemManager;

        protected IWorldContext  mWorldContext;

        [SetUp]
        public void Init()
        {
            mWorldContext = Substitute.For<IWorldContext>();
            mWorldContext.GetEntitiesWithAll().ReturnsForAnyArgs(new List<EntityId> { });

            mSystemManager = new SystemManager(mWorldContext);
        }
        
        [Test]
        public void TestRegisterInitSystem_PassNullArgument_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                mSystemManager.RegisterSystem((IInitSystem)null);
            });
        }

        [Test]
        public void TestRegisterInitSystem_PassCorrectReference_ReturnsUintHandle()
        {
            var systemMock = Substitute.For<IInitSystem>();

            Assert.DoesNotThrow(() =>
            {
                SystemId registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                Assert.AreEqual((uint)registeredSystemId, (uint)(E_SYSTEM_TYPE.ST_INIT) << 29);
            });
        }

        [Test]
        public void TestRegisterSystem_RegisterMixedInitAndUpdateSystems_ReturnsCorrectUintHandles()
        {
            Assert.DoesNotThrow(() =>
            {
                Random randomGenerator = new Random();

                SystemId currSystemId = (SystemId)0x0;

                int currNumOfRegisteredInitSystems   = 0;
                int currNumOfRegisteredUpdateSystems = 0;

                HashSet<SystemId> registeredSystemsIds = new HashSet<SystemId>();

                for (int i = 0; i < 10; ++i)
                { 
                    if (randomGenerator.Next(0, 2) > 0)
                    {
                        currSystemId = mSystemManager.RegisterSystem(Substitute.For<IInitSystem>());
                        
                        Assert.AreEqual((currNumOfRegisteredInitSystems << 16) | i | (((uint)E_SYSTEM_TYPE.ST_INIT) << 29), (uint)currSystemId);

                        ++currNumOfRegisteredInitSystems;
                    }
                    else
                    {
                        currSystemId = mSystemManager.RegisterSystem(Substitute.For<IUpdateSystem>());

                        Assert.AreEqual((currNumOfRegisteredUpdateSystems << 16) | i | (((uint)E_SYSTEM_TYPE.ST_UPDATE) << 29), (uint)currSystemId);

                        ++currNumOfRegisteredUpdateSystems;
                    }

                    // all identifiers should be unique no matter of which type the system is
                    Assert.IsFalse(registeredSystemsIds.Contains(currSystemId));

                    registeredSystemsIds.Add(currSystemId);
                }
            });
        }

        [Test]
        public void TestUnregisterSystem_PassSystemThatDoesntRegistered_ThrowsException()
        {
            Assert.Throws<InvalidIdentifierException>(() =>
            {
                mSystemManager.UnregisterSystem((SystemId)0);
            });
        }

        [Test]
        public void TestUnregisterSystem_PassCorrectId_UnregistersSpecifiedSystem()
        {
            var systemMock = Substitute.For<IUpdateSystem>();

            Assert.DoesNotThrow(() =>
            {
                SystemId registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                mSystemManager.UnregisterSystem(registeredSystemId);
            });
        }

        [Test]
        public void TestActivateSystem_PassInvalidId_ThrowsException()
        {
            Assert.Throws<InvalidIdentifierException>(() =>
            {
                mSystemManager.ActivateSystem((SystemId)2);
            });
        }

        [Test]
        public void TestActivateSystem_ActivateAlreadyActiveSystem_DoesNothing()
        {
            var systemMock = Substitute.For<IUpdateSystem>();

            Assert.DoesNotThrow(() =>
            {
                SystemId registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                // system's identifier should remain the same
                Assert.AreEqual(registeredSystemId, mSystemManager.ActivateSystem(registeredSystemId));
            });
        }

        [Test]
        public void TestActivateSystem_ActivateDeactivatedSystem_ActivatesGivenSystem()
        {
            var systemMock = Substitute.For<IUpdateSystem>();

            Assert.DoesNotThrow(() =>
            {
                SystemId registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                SystemId deactivatedSystemId = mSystemManager.DeactivateSystem(registeredSystemId);
                SystemId newSystemId = mSystemManager.ActivateSystem(registeredSystemId);

                Assert.AreNotEqual(newSystemId, deactivatedSystemId);
            });
        }

        [Test]
        public void TestRegisterSystem_TryRegisterMultiTaskSystem_RegisterSameReferenceAsMultiSystem()
        {
            var systemMock = Substitute.For<IUpdateSystem, IInitSystem>();

            Assert.DoesNotThrow(() =>
            {
                SystemId initSystemId   = mSystemManager.RegisterSystem(systemMock as IInitSystem);
                SystemId updateSystemId = mSystemManager.RegisterSystem(systemMock);

                Assert.AreNotEqual(initSystemId, updateSystemId);
                Assert.AreEqual(((uint)E_SYSTEM_TYPE.ST_INIT << 29), (uint)initSystemId);
                Assert.AreEqual(((uint)E_SYSTEM_TYPE.ST_UPDATE << 29), (uint)updateSystemId);
            });
        }

        [Test]
        public void TestUpdate_CreateEntityWithTwoComponents_EntityPassedOnceInReactiveSystem()
        {
            Assert.DoesNotThrow(() =>
            {
                mSystemManager.RegisterSystem(new PureReactiveSystemAdapter(mWorldContext,
                                                      entity => true, 
                                                      (world, entities, dt) => { Assert.AreEqual(1, entities.Count); }));

                // emulate an event when we create two components for same entity
                var listener = mSystemManager as IEventListener<TNewComponentAddedEvent>;

                EntityId id = (EntityId)1;

                listener.OnEvent(new TNewComponentAddedEvent { mOwnerId = id });
                listener.OnEvent(new TNewComponentAddedEvent { mOwnerId = id });

                mSystemManager.Update(0.0f);
            });
        }
    }
}
