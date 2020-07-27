using NUnit.Framework;
using TinyECS.Interfaces;
using TinyECS.Impls;
using NSubstitute;
using System;

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
            Assert.AreEqual(newEntity.Id, (EntityId)0);
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
                Assert.AreEqual(newEntity.Id, (EntityId)i);
            }

            /// remove some of entities
            Assert.IsTrue(mEntityManager.DestroyEntity((EntityId)2));
            Assert.IsTrue(mEntityManager.DestroyEntity((EntityId)3));

            /// recreate them
            newEntity = mEntityManager.CreateEntity();
            Assert.NotNull(newEntity);
            Assert.AreEqual(newEntity.Id, (EntityId)2);

            newEntity = mEntityManager.CreateEntity();
            Assert.NotNull(newEntity);
            Assert.AreEqual(newEntity.Id, (EntityId)3);
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

        [Test]
        public void TestDestroyEntity_TryToDestroyUnexistedEntity_ReturnsFalse()
        {
            Assert.IsFalse(mEntityManager.DestroyEntity(0));
        }

        [Test]
        public void TestDestroyEntity_DestroyExistingEntity_ReturnsTrueAndDestroyEntity()
        {
            var entity = mEntityManager.CreateEntity();

            Assert.IsTrue(mEntityManager.DestroyEntity(entity.Id));

            Assert.Throws<EntityDoesntExistException>(() =>
            {
                Assert.IsNull(mEntityManager.GetEntityById(entity.Id));
            });
        }

        [Test]
        public void TestNumOfActiveEntities_ThereAreNoEntitiesInWorld_ReturnsZero()
        {
            Assert.AreEqual(0, mEntityManager.NumOfActiveEntities);
        }

        [Test]
        public void TestNumOfActiveEntities_ThereAreFewEntitiesExistInWorld_ReturnsNumberOfEntities()
        {
            uint expectedNumOfEntities = 3;

            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < expectedNumOfEntities; ++i)
                {
                    Assert.IsNotNull(mEntityManager.CreateEntity());

                    Assert.AreEqual(i + 1, mEntityManager.NumOfActiveEntities);
                }
            });
        }

        [Test]
        public void TestNumOfActiveEntities_ThereAreFewEntitiesExistAndFewIsRemoved_ReturnsNumberOfEntities()
        {
            uint expectedNumOfEntities        = 3;
            uint expectedNumOfDeletedEntities = 2;

            Assert.DoesNotThrow(() =>
            {
                // preallocate entities in the world's context
                for (int i = 0; i < expectedNumOfEntities; ++i)
                {
                    Assert.IsNotNull(mEntityManager.CreateEntity());

                    Assert.AreEqual(i + 1, mEntityManager.NumOfActiveEntities);
                }

                // delete some entities
                for (uint i = 0; i < expectedNumOfDeletedEntities; ++i)
                {
                    EntityId id = (EntityId)i;

                    mEntityManager.DestroyEntity(id);
                }

                Assert.AreEqual(expectedNumOfEntities - expectedNumOfDeletedEntities, mEntityManager.NumOfActiveEntities);
            });
        }

        [Test]
        public void TestNumOfReusableEntities_InvokedOnEmptyEntityManager_ReturnsZero()
        {
            Assert.DoesNotThrow(() =>
            {
                Assert.AreEqual(0, mEntityManager.NumOfReusableEntities);
            });
        }

        [Test]
        public void TestNumOfReusableEntities_InvokedWhenTwoEntitiesWereCreated_ReturnsZero()
        {
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < 2; ++i)
                {
                    Assert.IsNotNull(mEntityManager.CreateEntity());
                }

                Assert.AreEqual(0, mEntityManager.NumOfReusableEntities);
            });
        }

        [Test]
        public void TestNumOfReusableEntities_InvokedWhenFewEntitiesWereCreatedAndFewDeleted_ReturnsLatterValue()
        {
            Assert.DoesNotThrow(() =>
            {
                uint numOfCreatedEntities   = 5;
                uint numOfDestroyedEntities = 2;
                
                for (int i = 0; i < numOfCreatedEntities; ++i)
                {
                    Assert.IsNotNull(mEntityManager.CreateEntity());
                }

                for (uint i = 0; i < numOfDestroyedEntities; ++i)
                {
                    mEntityManager.DestroyEntity((EntityId)i);
                }

                Assert.AreEqual(numOfDestroyedEntities, mEntityManager.NumOfReusableEntities);
            });
        }
    }
}
