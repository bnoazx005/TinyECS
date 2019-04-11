using System;
using TinyECS.Impls;
using TinyECS.Interfaces;


namespace SandboxProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWorldContext worldContext = new WorldContext(new EntityManager(new ComponentManager()));
            
            ISystemManager systemManager = new SystemManager(worldContext);

            systemManager.RegisterInitSystem(new PureInitSystemAdapter(worldContext, (world) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("call Init()");
            }));

            systemManager.RegisterUpdateSystem(new PureUpdateSystemAdapter(worldContext, (world, dt) =>
            {
                // worldContext's variable is available here
                Console.WriteLine("call Update(float)");
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
