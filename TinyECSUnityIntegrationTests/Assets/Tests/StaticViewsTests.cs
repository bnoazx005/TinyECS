using NUnit.Framework;
using System.Collections;
using TinyECSUnityIntegration.Impls;
using UnityEngine;
using UnityEngine.TestTools;


[TestFixture]
public class StaticViewsTests
{
    [UnityTest]
    public IEnumerator TestPreInit_CreateNewView_SuccessfullyInvokesPreInitAndCreateRequestEntity()
    {
        // This execution order imitates scenes loading, for dynamic objects the order doesn't make sense
        var view = TestStaticView.Create();
        var controller = TestController.Create();

        yield return new WaitForEndOfFrame();

        Assert.IsNotNull(controller);
        Assert.IsNotNull(view);

        var worldContext = controller.WorldContext;

        Assert.IsNotNull(worldContext);
        
        var registerRequestsEntity = worldContext.GetSingleEntityWithAll(typeof(TOnViewWaitForInitEventComponent));
        Assert.IsNotNull(registerRequestsEntity);

        Assert.DoesNotThrow(() =>
        {
            Assert.AreSame(view, registerRequestsEntity.GetComponent<TOnViewWaitForInitEventComponent>().mView);
        });

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(registerRequestsEntity.Id, view.LinkedEntityId);
    }

    [UnityTest]
    public IEnumerator TestRegisterSubscriptions_CreateNewView_InvokesRegisterSubscriptions()
    {
        bool hasRegisterSubscriptionsBeenInvoked = false;

        // This execution order imitates scenes loading, for dynamic objects the order doesn't make sense
        var view = TestStaticView.Create(() =>
        {
            hasRegisterSubscriptionsBeenInvoked = true;
        });
        var controller = TestController.Create();

        yield return new WaitForEndOfFrame();

        Assert.IsNotNull(controller);
        Assert.IsNotNull(view);

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.IsTrue(hasRegisterSubscriptionsBeenInvoked);
    }

    [UnityTest]
    public IEnumerator TestRegisterViewSystem_CreateNewView_SystemRegistersItAndRemoveRequestEntity()
    {
        // This execution order imitates scenes loading, for dynamic objects the order doesn't make sense
        var view = TestStaticView.Create();
        var controller = TestController.Create();

        yield return new WaitForEndOfFrame();

        Assert.IsNotNull(controller);
        Assert.IsNotNull(view);

        var worldContext = controller.WorldContext;

        Assert.IsNotNull(worldContext);
        Assert.IsNotNull(worldContext.GetSingleEntityWithAll(typeof(TOnViewWaitForInitEventComponent)));
        
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.IsNull(worldContext.GetSingleEntityWithAll(typeof(TOnViewWaitForInitEventComponent)));

        var viewEntity = worldContext.GetSingleEntityWithAll(typeof(TViewComponent));
        Assert.IsNotNull(viewEntity);
        Assert.AreEqual(viewEntity.Id, view.LinkedEntityId);
    }
}
