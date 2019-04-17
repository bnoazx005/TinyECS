using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// abstract class BaseReactiveSystem
    /// 
    /// The class is a common implementation for all reactive systems that are used within
    /// the framework. If you need to implement one just derive it from this class.
    /// </summary>

    public abstract class BaseReactiveSystem: IReactiveSystem
    {
        public BaseReactiveSystem()
        {
        }

        /// <summary>
        /// The method register the system within the given system manager based on the type
        /// </summary>
        /// <param name="systemManager">A reference to ISystemManager implementation</param>

        public void RegisterItself(ISystemManager systemManager)
        {
            systemManager?.RegisterReactiveSystem(this);
        }

        /// <summary>
        /// The method filters input list of entities based on its internal predicate's implementation
        /// </summary>
        /// <param name="entity">An input entity</param>
        /// <returns>The method should return true to pass the entity, false in other cases</returns>

        public abstract bool Filter(IEntity entity);

        /// <summary>
        /// The method is called if some entity was changed and passed a filter of the system
        /// </summary>
        /// <param name="entities">A list of entities that are passed a filter of the system</param>
        /// <param name="deltaTime">A time which is elapsed from the previous frame</param>

        public abstract void Update(List<IEntity> entities, float deltaTime);
    }
}
