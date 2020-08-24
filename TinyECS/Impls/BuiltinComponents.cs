using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// struct TDisposableComponent
    /// 
    /// The structure is a component-flag that makes an entity on to which it is assigned disposable one
    /// </summary>

    public struct TDisposableComponent: IComponent { }


    /// <summary>
    /// struct TEntityLifetimeComponent
    /// 
    /// This structure should be used in tie with TDisposableComponent to keep entity for a few extra frames
    /// </summary>

    public struct TEntityLifetimeComponent: IComponent
    {
        public uint mCounter; ///< A number of frames to live
    }
}
