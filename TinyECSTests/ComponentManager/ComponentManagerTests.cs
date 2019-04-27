using NSubstitute;
using NUnit.Framework;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace TinyECSTests
{
    [TestFixture]
    public class ComponentManagerTests
    {
        protected IComponentManager mComponentManager;

        protected struct TTestComponent: IComponent
        {
            public int mValue;
        }

        protected struct TAnotherComponent: IComponent
        {
        }

        [SetUp]
        public void Init()
        {
            mComponentManager = new ComponentManager(Substitute.For<IEventManager>());
        }

        [Test]
        public void TestAddComponent_AddComponent_ReturnsComponentsValue()
        {
            Assert.DoesNotThrow(() =>
            {
                mComponentManager.AddComponent<TTestComponent>(0);
            });
        }

        [Test]
        public void TestAddComponent_CreateNewComponentAndUpdateItsValueLater_UpdateExistingComponent()
        {
            Assert.DoesNotThrow(() =>
            {
                mComponentManager.AddComponent(0, new TTestComponent { mValue = 1 });

                var retrievedComponent = mComponentManager.GetComponent<TTestComponent>(0);

                Assert.AreEqual(retrievedComponent.mValue, 1);

                mComponentManager.AddComponent(0, new TTestComponent { mValue = 2 });

                retrievedComponent = mComponentManager.GetComponent<TTestComponent>(0);

                Assert.AreEqual(retrievedComponent.mValue, 2);
            });
        }

        [Test]
        public void TestGetComponent_GetComponentThatDoesntExist_ThrowsException()
        {
            mComponentManager.AddComponent<TTestComponent>(0);

            Assert.Throws<ComponentDoesntExistException>(() =>
            {
                var anotherComponent = mComponentManager.GetComponent<TAnotherComponent>(0);
            });
        }

        [Test]
        public void TestGetComponent_GetComponentOfEntityThatDoesntExist_ThrowsException()
        {
            Assert.Throws<EntityDoesntExistException>(() =>
            {
                var anotherComponent = mComponentManager.GetComponent<TAnotherComponent>(20);
            });
        }

        [Test]
        public void TestAddGetComponentMethods_AddNewComponentRetriveItsValue_ReturnsCorrectResult()
        {
            uint entityId = 1;

            int expectedValue = 42;

            Assert.DoesNotThrow(() =>
            {
                mComponentManager.AddComponent<TTestComponent>(entityId, new TTestComponent { mValue = expectedValue });

                TTestComponent testComponent = mComponentManager.GetComponent<TTestComponent>(entityId);

                Assert.AreEqual(testComponent.mValue, expectedValue);
            });
        }

        [Test]
        public void TestRemoveComponent_RemoveComponentOfEntityThatDoesntExist_ThrowsException()
        {
            uint entityId = 42;

            Assert.Throws<EntityDoesntExistException>(() =>
            {
                mComponentManager.RemoveComponent<TTestComponent>(entityId);
            });
        }

        [Test]
        public void TestRemoveComponent_RemoveComponentThatDoesntExist_ThrowsException()
        {
            uint entityId = 4;
            
            Assert.Throws<ComponentDoesntExistException>(() =>
            {
                // add test component
                mComponentManager.AddComponent<TTestComponent>(entityId, new TTestComponent { mValue = 42 });

                // but try to remove TAnotherComponent
                mComponentManager.RemoveComponent<TAnotherComponent>(entityId);
            });
        }

        [Test]
        public void TestRemoveComponent_RemoveComponentThatExists_ComponentManagerStaysInConsistentState()
        {
            uint entityId = 5;

            Assert.DoesNotThrow(() =>
            {
                // add test component
                mComponentManager.AddComponent<TTestComponent>(entityId, new TTestComponent { mValue = 42 });

                mComponentManager.RemoveComponent<TTestComponent>(entityId);
            });

            Assert.Throws<ComponentDoesntExistException>(() =>
            {
                var testComponent = mComponentManager.GetComponent<TTestComponent>(entityId);
            });
        }

        [Test]
        public void TestHasComponent_InvokeTheMethodOnEntityThatDoesntExist_ThrowsException()
        {
            Assert.Throws<EntityDoesntExistException>(() =>
            {
                mComponentManager.HasComponent<TTestComponent>(0);
            });
        }

        [Test]
        public void TestHasComponent_InvokeForEntityWithExistingComponent_ReturnsTrue()
        {
            uint entityId = 0;

            Assert.DoesNotThrow(() =>
            {
                // create a component
                mComponentManager.AddComponent<TTestComponent>(entityId);

                Assert.True(mComponentManager.HasComponent<TTestComponent>(entityId));
            });
        }

        [Test]
        public void TestHasComponent_InvokeForEntityWithoutComponent_ReturnsFalse()
        {
            uint entityId = 0;

            Assert.DoesNotThrow(() =>
            {
                // create a component but not one which will be checked
                mComponentManager.AddComponent<TAnotherComponent>(entityId);

                Assert.IsFalse(mComponentManager.HasComponent<TTestComponent>(entityId));
            });
        }
    }
}
