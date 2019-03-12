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
            mComponentManager = new ComponentManager();
        }

        [Test]
        public void TestAddComponent_AddComponent_ReturnsComponentsValue()
        {
            Assert.DoesNotThrow(() =>
            {
                TTestComponent component = mComponentManager.AddComponent<TTestComponent>(0);

                Assert.AreEqual(component, default(TTestComponent));
            });
        }

        [Test]
        public void TestGetComponent_GetComponentThatDoesntExist_ThrowsException()
        {
            TTestComponent component = mComponentManager.AddComponent<TTestComponent>(0);

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
    }
}
