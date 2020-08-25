using NUnit.Framework;
using System;
using System.Collections;
using TinyECSUnityIntegration.Impls;
using UnityEngine;
using UnityEngine.TestTools;


[TestFixture]
public class DependencyInjectorTests
{
    [UnityTest]
    public IEnumerator TestInit_CreateViewWithPlaneHierarchy_InvokesInit()
    {
        TestController.Create();

        yield return new WaitForEndOfFrame();

        bool isInitialized = false;

        GameObject viewGO = new GameObject("TestStaticView");

        viewGO.AddComponent<TestStaticView>().OnRegister = () =>
        {
            isInitialized = true;
        };

        var dependencyInjector = viewGO.AddComponent<DependencyInjector>();
        dependencyInjector.Init();

        yield return null;

        Assert.IsTrue(isInitialized);
    }

    // Test to check issue #30 https://github.com/bnoazx005/TinyECS/issues/30

    [UnityTest]
    public IEnumerator TestInit_CreateViewWithNestedSubviews_CorrectlyInitializesAllViews()
    {
        TestController.Create();

        yield return new WaitForEndOfFrame();

        uint actualCounter   = 0;
        uint expectedCounter = 3;

        Func<Transform, GameObject> addView = (parent) =>
        {
            GameObject go = new GameObject();

            go.AddComponent<TestStaticView>().OnRegister = () =>
            {
                actualCounter++;
            };

            go.transform.SetParent(parent);

            return go;
        };

        /*
         * The code below creates the following tree of GameObjects
         * 
         * View
         * |- SubView
         *    |-NestedView
         * -------
         */

        GameObject go1 = addView(null);
        GameObject go2 = addView(go1.transform);
        GameObject go3 = addView(go2.transform);

        var dependencyInjector = go1.AddComponent<DependencyInjector>();
        dependencyInjector.Init();

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(expectedCounter, actualCounter);
    }
}
