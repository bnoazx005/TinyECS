namespace TinyECS.Interfaces
{
    /// <summary>
    /// interface ISystem
    /// 
    /// The interface describes the most common functionality that all
    /// systems should implement
    /// </summary>

    public interface ISystem
    {
        /// <summary>
        /// The method register the system within the given system manager based on the type
        /// </summary>
        /// <param name="systemManager">A reference to ISystemManager implementation</param>

        void RegisterItself(ISystemManager systemManager);
    }


    /// <summary>
    /// interface IInitSystem
    /// 
    /// The interface represents a type of systems that are used only on initialization step of an application
    /// </summary>

    public interface IInitSystem: ISystem
    {
        /// <summary>
        /// The method is called once on initialization step
        /// </summary>

        void Init();
    }


    /// <summary>
    /// interface IUpdateSystem
    /// 
    /// The interface represents a type of systems that should be updated every frame
    /// </summary>

    public interface IUpdateSystem: ISystem
    {
        /// <summary>
        /// The method is called every frame
        /// </summary>
        /// <param name="deltaTime">A time which is elapsed from the previous frame</param>

        void Update(float deltaTime);
    }
}
