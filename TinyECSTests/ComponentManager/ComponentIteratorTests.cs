using NUnit.Framework;
using System;
using System.Collections.Generic;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace TinyECSTests
{
    [TestFixture]
    public class ComponentIteratorTests
    {
        protected struct TComponentA: IComponent
        {
            public int mValue;
        }

        protected struct TComponentB: IComponent
        {
        }

        protected struct TComponentC: IComponent
        {
        }


        [Test]
        public void TestConstructor_PassCorrectInputData_CreatesCorrectIterator()
        {
            var entityComponentsTable = new Dictionary<Type, int>();

            var componentsHashesTable = new Dictionary<Type, int>();

            var components = new List<IList<IComponent>>();

            Assert.DoesNotThrow(() =>
            {
                IComponentIterator iter = new ComponentManager.ComponentIterator(entityComponentsTable, componentsHashesTable, components);

                Assert.IsNotNull(iter);
            });
        }

        [Test]
        public void TestGet_InvokedOnEmptyComponentsMatrix_ThrowsException()
        {
            var entityComponentsTable = new Dictionary<Type, int>();

            var componentsHashesTable = new Dictionary<Type, int>();

            var components = new List<IList<IComponent>>();

            Assert.Throws<InvalidIteratorException>(() =>
            {
                IComponentIterator iter = new ComponentManager.ComponentIterator(entityComponentsTable, componentsHashesTable, components);

                Assert.IsNotNull(iter);

                iter.Get<TComponentA>();
            });
        }

        [Test]
        public void TestGet_InvokedOnCorrectIterator_ReturnsComponentValue()
        {
            var entityComponentsTable = new Dictionary<Type, int>
            {
                { typeof(TComponentA), 0 }
            };

            var componentsHashesTable = new Dictionary<Type, int>
            {
                { typeof(TComponentA), 0 }
            };

            var components = new List<IList<IComponent>>
            {
                new List<IComponent>
                {
                    new TComponentA() { mValue = 42 }
                }
            };

            Assert.Throws<InvalidIteratorException>(() =>
            {
                IComponentIterator iter = new ComponentManager.ComponentIterator(entityComponentsTable, componentsHashesTable, components);

                Assert.IsNotNull(iter);

                TComponentA componentValue = iter.Get<TComponentA>();

                Assert.AreEqual(42, componentValue.mValue);
            });
        }

        [Test]
        public void TestMoveNext_InvokedOnCorrectData_CorrectlyEnumeratesComponents()
        {
            var componentsPackageList = new List<Type>
            {
                typeof(TComponentA),
                typeof(TComponentB),
                typeof(TComponentC)
            };

            var entityComponentsTable = new Dictionary<Type, int>();

            foreach (var componentType in componentsPackageList)
            {
                entityComponentsTable.Add(componentType, 0);
            }

            var componentsHashesTable = new Dictionary<Type, int>();

            for (int i = 0; i < componentsPackageList.Count; ++i)
            {
                componentsHashesTable.Add(componentsPackageList[i], i);
            }

            var components = new List<IList<IComponent>>
            {
                new List<IComponent>
                {
                    new TComponentA() { mValue = 42 }
                },
                new List<IComponent>
                {
                    new TComponentB() { }
                },
                new List<IComponent>
                {
                    new TComponentC() { }
                },
            };

            Assert.DoesNotThrow(() =>
            {
                IComponentIterator iter = new ComponentManager.ComponentIterator(entityComponentsTable, componentsHashesTable, components);

                Assert.IsNotNull(iter);

                int visitedComponentsCount = 0;

                while (iter.MoveNext())
                {
                    IComponent currComponent = iter.Get();

                    Assert.AreEqual(componentsPackageList[visitedComponentsCount], currComponent.GetType());

                    ++visitedComponentsCount;
                }

                Assert.AreEqual(componentsPackageList.Count, visitedComponentsCount);
            });
        }
    }
}
