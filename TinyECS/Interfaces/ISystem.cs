using System.Collections.Generic;


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


    /// <summary>
    /// interface IReactiveSystem
    /// 
    /// The interface describes a functionality of a systems that are updated only specific
    /// set of entities that were changed and passed internal filters
    /// </summary>

    public interface IReactiveSystem: ISystem
    {
        /// <summary>
        /// The method filters input list of entities based on its internal predicate's implementation
        /// </summary>
        /// <param name="entity">An input entity</param>
        /// <returns>The method should return true to pass the entity, false in other cases</returns>

        bool Filter(IEntity entity);

        /// <summary>
        /// The method is called if some entity was changed and passed a filter of the system
        /// </summary>
        /// <param name="entities">A list of entities that are passed a filter of the system</param>
        /// <param name="deltaTime">A time which is elapsed from the previous frame</param>

        void Update(List<IEntity> entities, float deltaTime);
    }
}
