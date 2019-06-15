using System.Collections.Generic;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    /// <summary>
    /// abstract class PureSystemAdapter
    /// 
    /// The class is an adapter between lambdas which are pure systems and
    /// base implementations of systems that are used within the framework
    /// </summary>

    public abstract class PureSystemAdapter: ISystem
    {
        /// <summary>
        /// The method register the system within the given system manager based on the type
        /// </summary>
        /// <param name="systemManager">A reference to ISystemManager implementation</param>

        public abstract void RegisterItself(ISystemManager systemManager);
    }


    /// <summary>
    /// class PureInitSystemAdapther
    /// 
    /// The class is a type of an adapter which is used to utilize lambdas to initialize
    /// states of entities before the main loop of an application
    /// </summary>

    public class PureInitSystemAdapter: PureSystemAdapter, IInitSystem
    {
        public delegate void InitLambdaSystem(IWorldContext worldContext);

        protected InitLambdaSystem mInitLambdaSystem;

        protected IWorldContext    mWorldContext;

        public PureInitSystemAdapter(IWorldContext worldContext, InitLambdaSystem lambdaSystem)
        {
            mWorldContext = worldContext ?? throw new System.ArgumentNullException("worldContext");

            mInitLambdaSystem = lambdaSystem ?? throw new System.ArgumentNullException("lambdaSystem");
        }

        /// <summary>
        /// The method register the system within the given system manager based on the type
        /// </summary>
        /// <param name="systemManager">A reference to ISystemManager implementation</param>

        public override void RegisterItself(ISystemManager systemManager)
        {
            if (systemManager == null)
            {
                throw new System.ArgumentNullException("systemManager");
            }

            systemManager.RegisterSystem(this);
        }

        public void Init()
        {
            mInitLambdaSystem?.Invoke(mWorldContext);
        }
    }


    /// <summary>
    /// class PureUpdateSystemAdapther
    /// 
    /// The class is a type of an adapter which is used to utilize lambdas to update entities
    /// within the main loop
    /// </summary>

    public class PureUpdateSystemAdapter : PureSystemAdapter, IUpdateSystem
    {
        public delegate void UpdateLambdaSystem(IWorldContext worldContext, float dt);

        protected UpdateLambdaSystem mUpdateLambdaSystem;

        protected IWorldContext      mWorldContext;

        public PureUpdateSystemAdapter(IWorldContext worldContext, UpdateLambdaSystem lambdaSystem)
        {
            mWorldContext = worldContext ?? throw new System.ArgumentNullException("worldContext");

            mUpdateLambdaSystem = lambdaSystem ?? throw new System.ArgumentNullException("lambdaSystem");
        }

        /// <summary>
        /// The method register the system within the given system manager based on the type
        /// </summary>
        /// <param name="systemManager">A reference to ISystemManager implementation</param>

        public override void RegisterItself(ISystemManager systemManager)
        {
            if (systemManager == null)
            {
                throw new System.ArgumentNullException("systemManager");
            }

            systemManager.RegisterSystem(this);
        }

        public void Update(float dt)
        {
            mUpdateLambdaSystem?.Invoke(mWorldContext, dt);
        }
    }


    /// <summary>
    /// class PureReactiveSystemAdapter
    /// 
    /// The class is a type of an adapter which is used to utilize lambdas to implement
    /// reactive system that processes changing entities
    /// </summary>

    public class PureReactiveSystemAdapter: PureSystemAdapter, IReactiveSystem
    {
        public delegate bool ReactiveSystemFilter(IEntity entity);

        public delegate void ReactiveLambdaSystem(IWorldContext worldContext, List<IEntity> entities, float dt);
        
        protected ReactiveLambdaSystem mReactiveLambdaSystem;

        protected ReactiveSystemFilter mReactiveSystemFilter;

        protected IWorldContext        mWorldContext;

        public PureReactiveSystemAdapter(IWorldContext worldContext, ReactiveSystemFilter systemFilter, ReactiveLambdaSystem lambdaSystem)
        {
            mWorldContext = worldContext ?? throw new System.ArgumentNullException("worldContext");

            mReactiveLambdaSystem = lambdaSystem ?? throw new System.ArgumentNullException("lambdaSystem");

            mReactiveSystemFilter = systemFilter ?? throw new System.ArgumentNullException("systemFilter");
        }

        /// <summary>
        /// The method register the system within the given system manager based on the type
        /// </summary>
        /// <param name="systemManager">A reference to ISystemManager implementation</param>

        public override void RegisterItself(ISystemManager systemManager)
        {
            if (systemManager == null)
            {
                throw new System.ArgumentNullException("systemManager");
            }

            systemManager.RegisterSystem(this);
        }

        /// <summary>
        /// The method filters input list of entities based on its internal predicate's implementation
        /// </summary>
        /// <param name="entity">An input entity</param>
        /// <returns>The method should return true to pass the entity, false in other cases</returns>

        public bool Filter(IEntity entity)
        {
            return mReactiveSystemFilter?.Invoke(entity) ?? false;
        }

        /// <summary>
        /// The method is called if some entity was changed and passed a filter of the system
        /// </summary>
        /// <param name="entities">A list of entities that are passed a filter of the system</param>
        /// <param name="deltaTime">A time which is elapsed from the previous frame</param>

        public void Update(List<IEntity> entities, float deltaTime)
        {
            mReactiveLambdaSystem?.Invoke(mWorldContext, entities, deltaTime);
        }
    }
}
