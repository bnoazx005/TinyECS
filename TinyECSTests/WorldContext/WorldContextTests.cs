using NUnit.Framework;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace TinyECSTests
{
    /// <summary>
    /// class WorldContextTests
    /// 
    /// The class is a union of integration tests which are related with
    /// WorldContext class and its usage
    /// </summary>

    [TestFixture]
    public class WorldContextTests
    {
        protected IEntityManager mEntityManager;

        protected IWorldContext  mWorldContext;

        [SetUp]
        public void Init()
        {
            mEntityManager = new EntityManager(new ComponentManager());

            mWorldContext = new WorldContext(mEntityManager);
        }

        [Test]
        public void TestEntitiesCreation_Pass()
        {
            // new entities that are created in continuous manner
            // should have increasing identifiers with a delta equals to 1
            for (uint i = 0; i < 10; ++i)
            {
                Assert.AreEqual(i, mWorldContext.CreateEntity());
                Assert.AreEqual(i, mWorldContext.GetEntityById(i).Id);
            }


        }
    }
}
