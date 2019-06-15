using System;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace Tutorial01_HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWorldContext worldContext = new WorldContextFactory().CreateNewWorldInstance();

            ISystemManager systemManager = new SystemManager(worldContext);

            // register our classes that implement systems
            systemManager.RegisterSystem(new PrintHelloWorldSystem());
            systemManager.RegisterSystem(new ReactivePrintHelloWorldSystem());

            // another way of doing the same things is to use adapters and lambdas instead of classes
            systemManager.RegisterSystem(new PureInitSystemAdapter(worldContext, (world) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("PureInitSystem: Hello, World!");
            }));

            systemManager.RegisterSystem(new PureReactiveSystemAdapter(worldContext, 
                    entity => entity.HasComponent<THelloWorldComponent>(), 
                    (world, entities, dt) =>
                    {
                        // worldContext's variable is available here
                        Console.WriteLine("PureReactiveSystem: Hello, World!");
                    }));

            systemManager.Init();

            bool isRunning = true;

            while (isRunning)
            {
                if (Console.ReadLine() != string.Empty)
                {
                    uint entityId = worldContext.CreateEntity();

                    IEntity entity = worldContext.GetEntityById(entityId);

                    entity.AddComponent<THelloWorldComponent>();

                    isRunning = false;
                }

                systemManager.Update(0.0f);
            }

            Console.ReadKey();
        }
    }
}
