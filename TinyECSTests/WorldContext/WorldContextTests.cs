using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        protected struct TTestComponent: IComponent
        {
        }

        protected struct TAnotherComponent : IComponent
        {
        }

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

        [Test]
        public void TestGetEntitiesWithAll_PassNoArguments_ReturnsAllEntities()
        {
            List<uint> expectedEntities = new List<uint>();

            for (int i = 0; i < 5; ++i)
            {
                expectedEntities.Add(mWorldContext.CreateEntity());
            }

            Assert.DoesNotThrow(() =>
            {
                var actualEntities = mWorldContext.GetEntitiesWithAll();

                Assert.AreEqual(expectedEntities.Count, actualEntities.Count);

                actualEntities.Sort();
                expectedEntities.Sort();

                for (int i = 0; i < expectedEntities.Count; ++i)
                {
                    Assert.AreEqual(expectedEntities[i], actualEntities[i]);
                }
            });
        }

        [Test]
        public void TestGetEntitiesWithAll_PassSingleArgument_ReturnsEntitiesWithGivenComponent()
        {
            List<uint> expectedEntities = new List<uint>(); // contains only entities with TTestComponent's attached to them

            Random randomGenerator = new Random();

            for (int i = 0; i < 5; ++i)
            {
                var entity = mWorldContext.GetEntityById(mWorldContext.CreateEntity());

                if (randomGenerator.Next(0, 2) > 0)
                {
                    entity.AddComponent<TTestComponent>();

                    expectedEntities.Add(entity.Id);
                }
            }

            Assert.DoesNotThrow(() =>
            {
                var actualEntities = mWorldContext.GetEntitiesWithAll(typeof(TTestComponent));

                Assert.AreEqual(expectedEntities.Count, actualEntities.Count);

                actualEntities.Sort();
                expectedEntities.Sort();

                for (int i = 0; i < expectedEntities.Count; ++i)
                {
                    Assert.AreEqual(expectedEntities[i], actualEntities[i]);
                }
            });
        }

        [Test]
        public void TestGetEntitiesWithAll_PassFewArgument_ReturnsEntitiesThatHaveAllGivenComponents()
        {
            List<uint> expectedEntities = new List<uint>(); // contains only entities with TTestComponent's attached to them

            Random randomGenerator = new Random();

            for (int i = 0; i < 5; ++i)
            {
                var entity = mWorldContext.GetEntityById(mWorldContext.CreateEntity());

                if (randomGenerator.Next(0, 2) > 0)
                {
                    entity.AddComponent<TTestComponent>();
                    entity.AddComponent<TAnotherComponent>();

                    expectedEntities.Add(entity.Id);
                }
            }

            Assert.DoesNotThrow(() =>
            {
                var actualEntities = mWorldContext.GetEntitiesWithAll(typeof(TTestComponent), typeof(TAnotherComponent));

                Assert.AreEqual(expectedEntities.Count, actualEntities.Count);

                actualEntities.Sort();
                expectedEntities.Sort();

                for (int i = 0; i < expectedEntities.Count; ++i)
                {
                    Assert.AreEqual(expectedEntities[i], actualEntities[i]);
                }
            });
        }

        [Test]
        public void TestGetEntitiesWithAny_PassNoArguments_ReturnsEmptyArray()
        {
            List<uint> expectedEntities = new List<uint>();

            // create new entities;
            for (int i = 0; i < 5; ++i)
            {
                var entity = mWorldContext.GetEntityById(mWorldContext.CreateEntity());

                entity.AddComponent<TTestComponent>();
                entity.AddComponent<TAnotherComponent>();
            }

            Assert.DoesNotThrow(() =>
            {
                var actualEntities = mWorldContext.GetEntitiesWithAny();

                Assert.AreEqual(expectedEntities.Count, actualEntities.Count);
            });
        }

        [Test]
        public void TestGetEntitiesWithAny_PassTwoArgument_ReturnsEntitiesWithFirstOrSecondComponent()
        {
            List<uint> expectedEntities = new List<uint>();

            // create new entities;
            for (int i = 0; i < 5; ++i)
            {
                var entity = mWorldContext.GetEntityById(mWorldContext.CreateEntity());

                expectedEntities.Add(entity.Id);

                entity.AddComponent<TTestComponent>();
                entity.AddComponent<TAnotherComponent>();
            }

            Assert.DoesNotThrow(() =>
            {
                var actualEntities = mWorldContext.GetEntitiesWithAny(typeof(TTestComponent), typeof(TAnotherComponent));

                Assert.AreEqual(expectedEntities.Count, actualEntities.Count);

                actualEntities.Sort();
                expectedEntities.Sort();

                for (int i = 0; i < expectedEntities.Count; ++i)
                {
                    Assert.AreEqual(expectedEntities[i], actualEntities[i]);
                }
            });
        }
    }
}
