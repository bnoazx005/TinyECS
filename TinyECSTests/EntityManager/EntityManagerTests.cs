using NUnit.Framework;
using TinyECS.Interfaces;
using TinyECS.Impls;
using NSubstitute;


namespace TinyECSTests
{
    [TestFixture]
    public class EntityManagerTests
    {
        protected IEntityManager mEntityManager;

        [SetUp]
        public void Init()
        {
            var componentManagerMock = Substitute.For<IComponentManager>();
            
            mEntityManager = new EntityManager(componentManagerMock);
        }

        [TestCase]
        public void TestCreateEntity_PassNullArg_ReturnsEntityWithDefaultName()
        { 
            Assert.NotNull(mEntityManager);

            var newEntity = mEntityManager.CreateEntity(null);

            Assert.NotNull(newEntity);
            Assert.AreEqual(newEntity.Name, "Entity0");
            Assert.AreEqual(newEntity.Id, 0);
        }

        [TestCase]
        public void TestCreateEntity_ReusingEntities_ReturnsPreviouslyCreatedEntity()
        {
            Assert.NotNull(mEntityManager);

            IEntity newEntity = null;

            for (int i = 0; i < 5; ++i)
            {
                newEntity = mEntityManager.CreateEntity(null);

                Assert.NotNull(newEntity);
                Assert.AreEqual(newEntity.Name, $"Entity{i}");
                Assert.AreEqual(newEntity.Id, i);
            }

            /// remove some of entities
            Assert.IsTrue(mEntityManager.DestroyEntity(2));
            Assert.IsTrue(mEntityManager.DestroyEntity(3));

            /// recreate them
            newEntity = mEntityManager.CreateEntity();
            Assert.NotNull(newEntity);
            Assert.AreEqual(newEntity.Id, 2);

            newEntity = mEntityManager.CreateEntity();
            Assert.NotNull(newEntity);
            Assert.AreEqual(newEntity.Id, 3);
        }

        [Test]
        public void TestGetEntityById_PassInvalidEntityId_ThrowsException()
        {
            Assert.Throws<EntityDoesntExistException>(() =>
            {
                mEntityManager.GetEntityById(0);
            });
        }

        [Test]
        public void TestGetEntityById_PassCorrectExistingEntityId_ReturnsReferenceToIt()
        {
            Assert.DoesNotThrow(() =>
            {
                IEntity newEntity = mEntityManager.CreateEntity();

                Assert.AreSame(newEntity, mEntityManager.GetEntityById(newEntity.Id));
            });
        }
    }
}
