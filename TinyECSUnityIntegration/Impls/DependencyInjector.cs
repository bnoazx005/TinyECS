﻿using System;
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

        protected BaseView[]           mParentViews;

        protected bool                 mIsInitialized = false;

        public void Init()
        {
            var worldContext = _worldContextsManager?.WorldContext;

            if (mIsInitialized || worldContext == null)
            {
                return;
            }
            
            Array.ForEach(_parentViews, entity => entity?.PreInit(worldContext));
            
            mIsInitialized = true;
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

        protected BaseView[] _parentViews
        {
            get
            {
                if (mParentViews == null)
                {
                    mParentViews = GetComponentsInChildren<BaseView>();
                }

                return mParentViews;
            }
        }
    }
}
