using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;
using UnityEngine;
using UnityEngine.Events;


public class TestStaticView : BaseStaticView
{
    public UnityAction OnRegister;

    public override void RegisterSubscriptions(IEventManager eventManager, EntityId entityId)
    {
        OnRegister?.Invoke();
    }

    public static BaseView Create(UnityAction action = null)
    {
        GameObject go = new GameObject("TestStaticView");
        go.AddComponent<DependencyInjector>();

        TestStaticView view = go.AddComponent<TestStaticView>();
        view.OnRegister = action;

        return view;
    }
}