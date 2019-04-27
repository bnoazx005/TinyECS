using UnityEngine;


namespace TinyECSUnityIntegration.Impls
{
    /// <summary>
    /// class BaseStaticView
    /// 
    /// The class is a subtype of BaseView which should be used in cases when some view
    /// will be created in compile time and never be allocated dynamically in runtime
    /// </summary>

    [RequireComponent(typeof(DependencyInjector))]
    public abstract class BaseStaticView: BaseView
    {
    }
}
