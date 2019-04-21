using System;
using System.Collections.Generic;
using TinyECS.Impls;
using TinyECS.Interfaces;


public class PrintHelloWorldSystem : IInitSystem
{
    public void RegisterItself(ISystemManager systemManager)
    {
        systemManager?.RegisterInitSystem(this);
    }

    public void Init()
    {
        Console.WriteLine("PrintHelloWorldSystem: Hello, World!");
    }
}


public class ReactivePrintHelloWorldSystem : BaseReactiveSystem
{
    public override bool Filter(IEntity entity)
    {
        return entity.HasComponent<THelloWorldComponent>();
    }

    public override void Update(List<IEntity> entities, float deltaTime)
    {
        Console.WriteLine("ReactivePrintHelloWorldSystem: Hello, World!");
    }
}