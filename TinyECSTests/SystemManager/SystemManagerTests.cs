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

        [SetUp]
        public void Init()
        {
            IWorldContext worldContextMock = Substitute.For<IWorldContext>();

            mSystemManager = new SystemManager(worldContextMock);
        }
        
        [Test]
        public void TestRegisterInitSystem_PassNullArgument_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                mSystemManager.RegisterInitSystem(null);
            });
        }

        [Test]
        public void TestRegisterInitSystem_PassCorrectReference_ReturnsUintHandle()
        {
            var systemMock = Substitute.For<IInitSystem>();

            Assert.DoesNotThrow(() =>
            {
                uint registeredSystemId = mSystemManager.RegisterInitSystem(systemMock);

                Assert.AreEqual(registeredSystemId, 0);
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
                        currSystemId = mSystemManager.RegisterInitSystem(Substitute.For<IInitSystem>());
                        
                        Assert.AreEqual((currNumOfRegisteredInitSystems << 16) | i, currSystemId);

                        ++currNumOfRegisteredInitSystems;
                    }
                    else
                    {
                        currSystemId = mSystemManager.RegisterUpdateSystem(Substitute.For<IUpdateSystem>());

                        Assert.AreEqual((currNumOfRegisteredUpdateSystems << 16) | i, currSystemId);

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
                uint registeredSystemId = mSystemManager.RegisterUpdateSystem(systemMock);

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
                uint registeredSystemId = mSystemManager.RegisterUpdateSystem(systemMock);

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
                uint registeredSystemId = mSystemManager.RegisterUpdateSystem(systemMock);

                uint deactivatedSystemId = mSystemManager.DeactivateSystem(registeredSystemId);
                uint newSystemId = mSystemManager.ActivateSystem(registeredSystemId);

                Assert.AreNotEqual(newSystemId, deactivatedSystemId);
            });
        }
    }
}
