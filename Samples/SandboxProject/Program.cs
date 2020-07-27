using System;
using System.Diagnostics;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace SandboxProject
{
    public class Program
    {
        public struct TestComponent: IComponent
        {
        }

        public static void Main(string[] args)
        {
            IWorldContext worldContext = new WorldContextFactory().CreateNewWorldInstance();
            
            ISystemManager systemManager = new SystemManager(worldContext);

            systemManager.RegisterSystem(new PureInitSystemAdapter(worldContext, (world) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("call Init()");

                var e = worldContext.GetEntityById(worldContext.CreateEntity());

                e.AddComponent<TestComponent>();
            }));

            systemManager.RegisterSystem(new PureUpdateSystemAdapter(worldContext, (world, dt) =>
            {
                var entitiesArray = worldContext.GetEntitiesWithAll(typeof(TestComponent));

                // worldContext's variable is available here
                Console.WriteLine("call Update(float)");

            }));

            systemManager.RegisterSystem(new PureReactiveSystemAdapter(worldContext, entity => true, (world, entities, dt) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("call Update(entities, float)");
            }));

            systemManager.Init();

            for (int i = 0; i < 5; ++i)
            {
                IEntity entity = worldContext.GetEntityById(worldContext.CreateEntity());
                Debug.Assert(entity != null);

                entity.AddComponent<TestComponent>();
            }
            
            for (int i = 0; i < 10; ++i)
            {
                systemManager.Update(0.0f);
            }

            Console.ReadKey();
        }
    }
}
