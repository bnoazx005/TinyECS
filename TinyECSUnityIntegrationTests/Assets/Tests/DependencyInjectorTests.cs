using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TinyECSUnityIntegration.Impls;
using UnityEngine;
using UnityEngine.TestTools;


[TestFixture]
public class DependencyInjectorTests
{
    [Test]
    public IEnumerator TestInit_CreateViewWithPlaneHierarchy_InvokesInit()
    {
        bool isInitialized = false;

        GameObject viewGO = new GameObject("TestStaticView");

        viewGO.AddComponent<TestStaticView>().OnRegister = () =>
        {
            isInitialized = true;
        };

        var dependencyInjector = viewGO.AddComponent<DependencyInjector>();
        dependencyInjector.Init();

        Assert.IsTrue(isInitialized);
    }
}
