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
                uint registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                Assert.AreEqual(registeredSystemId, (byte)(E_SYSTEM_TYPE.ST_INIT) << 29);
            });
        }

        [Test]
        public void TestRegisterSystem_RegisterMixedInitAndUpdateSystems_ReturnsCorrectUintHandles()
        {
            Assert.DoesNotThrow(() =>
            {
                Random randomGenerator = new Random();

                uint currSystemId = 0x0;

                int currNumOfRegisteredInitSystems   = 0;
                int currNumOfRegisteredUpdateSystems = 0;

                HashSet<uint> registeredSystemsIds = new HashSet<uint>();

                for (int i = 0; i < 10; ++i)
                { 
                    if (randomGenerator.Next(0, 2) > 0)
                    {
                        currSystemId = mSystemManager.RegisterSystem(Substitute.For<IInitSystem>());
                        
                        Assert.AreEqual((currNumOfRegisteredInitSystems << 16) | i | (((byte)E_SYSTEM_TYPE.ST_INIT) << 29), currSystemId);

                        ++currNumOfRegisteredInitSystems;
                    }
                    else
                    {
                        currSystemId = mSystemManager.RegisterSystem(Substitute.For<IUpdateSystem>());

                        Assert.AreEqual((currNumOfRegisteredUpdateSystems << 16) | i | (((byte)E_SYSTEM_TYPE.ST_UPDATE) << 29), currSystemId);

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
                mSystemManager.UnregisterSystem(0);
            });
        }

        [Test]
        public void TestUnregisterSystem_PassCorrectId_UnregistersSpecifiedSystem()
        {
            var systemMock = Substitute.For<IUpdateSystem>();

            Assert.DoesNotThrow(() =>
            {
                uint registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                mSystemManager.UnregisterSystem(registeredSystemId);
            });
        }

        [Test]
        public void TestActivateSystem_PassInvalidId_ThrowsException()
        {
            Assert.Throws<InvalidIdentifierException>(() =>
            {
                mSystemManager.ActivateSystem(2);
            });
        }

        [Test]
        public void TestActivateSystem_ActivateAlreadyActiveSystem_DoesNothing()
        {
            var systemMock = Substitute.For<IUpdateSystem>();

            Assert.DoesNotThrow(() =>
            {
                uint registeredSystemId = mSystemManager.RegisterSystem(systemMock);

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
                uint registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                uint deactivatedSystemId = mSystemManager.DeactivateSystem(registeredSystemId);
                uint newSystemId = mSystemManager.ActivateSystem(registeredSystemId);

                Assert.AreNotEqual(newSystemId, deactivatedSystemId);
            });
        }

        [Test]
        public void TestRegisterSystem_TryRegisterMultiTaskSystem_RegisterSameReferenceAsMultiSystem()
        {
            var systemMock = Substitute.For<IUpdateSystem, IInitSystem>();

            Assert.DoesNotThrow(() =>
            {
                uint initSystemId   = mSystemManager.RegisterSystem(systemMock as IInitSystem);
                uint updateSystemId = mSystemManager.RegisterSystem(systemMock);

                Assert.AreNotEqual(initSystemId, updateSystemId);
                Assert.AreEqual(((byte)E_SYSTEM_TYPE.ST_INIT << 29), initSystemId);
                Assert.AreEqual(((byte)E_SYSTEM_TYPE.ST_UPDATE << 29), updateSystemId);
            });
        }

        [Test]
        public void TestUpdate_CreateEntityWithTwoComponents_EntityPassedOnceInReactiveSystem()
        {
            Assert.DoesNotThrow(() =>
            {
                mSystemManager.RegisterReactiveSystem(new PureReactiveSystemAdapter(mWorldContext,
                                                      entity => true, 
                                                      (world, entities, dt) => { Assert.AreEqual(1, entities.Count); }));

                // emulate an event when we create two components for same entity
                var listener = mSystemManager as IEventListener<TNewComponentAddedEvent>;
                listener.OnEvent(new TNewComponentAddedEvent { mOwnerId = 1 });
                listener.OnEvent(new TNewComponentAddedEvent { mOwnerId = 1 });

                mSystemManager.Update(0.0f);
            });
        }
    }
}
