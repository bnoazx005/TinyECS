using TinyECS.Interfaces;
using TinyECSUnityIntegration.Interfaces;
using UnityEngine;


namespace TinyECSUnityIntegration.Impls
{ 
    /// <summary>
    /// class DependencyInjector
    /// 
    /// The class is a utility class that injects reference to IWorldContext
    /// </summary>

    public class DependencyInjector: MonoBehaviour, IDependencyInjector
    {
        protected WorldContextsManager mWorldContextsManager;

        protected BaseView             mParentView;

        protected bool                 mIsInitialized = false;

        public void Init()
        {
            if (mIsInitialized)
            {
                return;
            }

            _parentView.PreInit(_worldContextsManager?.WorldContext);
            
            mIsInitialized = _parentView.WorldContext != null;
        }

        protected WorldContextsManager _worldContextsManager
        {
            get
            {
                if (mWorldContextsManager == null)
                {
                    mWorldContextsManager = FindObjectOfType<WorldContextsManager>();
                }

                return mWorldContextsManager;
            }
        }

        protected BaseView _parentView
        {
            get
            {
                if (mParentView == null)
                {
                    mParentView = GetComponentInParent<BaseView>();
                }

                return mParentView;
            }
        }
    }
}
