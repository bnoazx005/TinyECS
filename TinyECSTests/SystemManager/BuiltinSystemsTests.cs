using NUnit.Framework;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace TinyECSTests
{
    [TestFixture]
    public class BuiltinSystemsTests
    {
        protected ISystemManager mSystemManager;

        protected IWorldContext  mWorldContext;

        [SetUp]
        public void Init()
        {
            mWorldContext = new WorldContextFactory().CreateNewWorldInstance();

            mSystemManager = new SystemManager(mWorldContext);
        }

        [Test]
        public void TestDisposableEntitiesCollectorSystem_ThereAreNoDisposableEntitiesAtInput_DoNothing()
        {
            mSystemManager.RegisterSystem(new PureUpdateSystemAdapter(mWorldContext, BuiltinSystems.DisposableEntitiesCollectorSystem));

            int expectedNumOfDisposableEntities = 5;

            for (int i = 0; i < expectedNumOfDisposableEntities; ++i)
            {
                mWorldContext.CreateDisposableEntity();
            }

            Assert.AreEqual(expectedNumOfDisposableEntities, mWorldContext.GetEntitiesWithAll(typeof(TDisposableComponent)).Count);

            mSystemManager.Init();
            mSystemManager.Update(0.0f);

            Assert.AreEqual(0, mWorldContext.GetEntitiesWithAll(typeof(TDisposableComponent)).Count);
        }

        [Test]
        public void TestDisposableEntitiesCollectorSystem_DisposableEntitiesAreCreatedBeforeEachUpdate_CorrectlyDeleteThem()
        {
            mSystemManager.RegisterSystem(new PureUpdateSystemAdapter(mWorldContext, BuiltinSystems.DisposableEntitiesCollectorSystem));

            int expectedNumOfDisposableEntities = 5;
            
            mSystemManager.Init();

            for (int i = 0; i < expectedNumOfDisposableEntities; ++i)
            {
                mWorldContext.CreateDisposableEntity();
                mSystemManager.Update(0.0f);
            }

            Assert.AreEqual(0, mWorldContext.GetEntitiesWithAll(typeof(TDisposableComponent)).Count);
        }
    }
}
