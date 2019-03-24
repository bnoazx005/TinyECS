using NSubstitute;
using NUnit.Framework;
using System;
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
            mSystemManager = new SystemManager();
        }

        [Test]
        public void TestRegisterSystem_PassNullInsteadOfCorrectReference_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                mSystemManager.RegisterSystem(null);
            });
        }

        [Test]
        public void TestRegisterSystem_PassCorrectReference_ReturnsUintHandle()
        {
            var systemMock = Substitute.For<IUpdateSystem>();

            Assert.DoesNotThrow(() =>
            {
                uint registeredSystemId = mSystemManager.RegisterSystem(systemMock);
            });
        }

        [Test]
        public void TestRegisterSystem_PassSystemThatAlreadyRegistered_ReturnsHandleToRegisteredSystem()
        {
            var systemMock = Substitute.For<IUpdateSystem>();

            Assert.DoesNotThrow(() =>
            {
                uint registeredSystemId = mSystemManager.RegisterSystem(systemMock);

                uint duplicateSystemId = mSystemManager.RegisterSystem(systemMock);

                Assert.AreEqual(registeredSystemId, duplicateSystemId);
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
    }
}
