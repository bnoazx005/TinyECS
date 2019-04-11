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

            systemManager.RegisterInitSystem(this);
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

            systemManager.RegisterUpdateSystem(this);
        }

        public void Update(float dt)
        {
            mUpdateLambdaSystem?.Invoke(mWorldContext, dt);
        }
    }
}
