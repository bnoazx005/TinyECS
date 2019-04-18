using System;
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

            systemManager.RegisterInitSystem(new PureInitSystemAdapter(worldContext, (world) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("call Init()");

                var e = worldContext.GetEntityById(worldContext.CreateEntity());

                e.AddComponent<TestComponent>();
            }));

            systemManager.RegisterUpdateSystem(new PureUpdateSystemAdapter(worldContext, (world, dt) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("call Update(float)");
            }));

            systemManager.RegisterReactiveSystem(new PureReactiveSystemAdapter(worldContext, entity => true, (world, entities, dt) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("call Update(entities, float)");
            }));

            systemManager.Init();

            for (int i = 0; i < 10; ++i)
            {
                systemManager.Update(0.0f);
            }

            Console.ReadKey();
        }
    }
}
