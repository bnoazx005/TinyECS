using NUnit.Framework;
using TinyECS.Impls;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;


namespace TinyECSTests
{
    [TestFixture]
    public class RegisterViewSystemTests
    {
        protected IWorldContext   mWorldContext;

        protected ISystemManager  mSystemManager;

        protected IReactiveSystem mRegisterViewsSystem;

        [SetUp]
        public void Init()
        {
            mWorldContext = new WorldContextFactory().CreateNewWorldInstance();

            mSystemManager = new SystemManager(mWorldContext);

            mRegisterViewsSystem = new RegisterViewSystem(mWorldContext);

            mSystemManager.RegisterSystem(mRegisterViewsSystem);

            mSystemManager.Init();
        }

        [Test]
        public void TestUpdateMethodOfSystem_PassCorrectData_SuccessfullyProcessesData()
        {
            string entityName = "entity01";

            // prepare data
            var e = mWorldContext.GetEntityById(mWorldContext.CreateEntity(entityName));

            e.AddComponent<TOnViewWaitForInitEventComponent>();

            Assert.IsNotNull(e);
            Assert.AreEqual(e.Name, entityName);

            // execute system
            mSystemManager.Update(0.0f);

            // check results
            Assert.DoesNotThrow(() =>
            {
                Assert.IsFalse(e.HasComponent<TOnViewWaitForInitEventComponent>());
                Assert.IsTrue(e.HasComponent<TViewComponent>());
            });
        }
    }
}
