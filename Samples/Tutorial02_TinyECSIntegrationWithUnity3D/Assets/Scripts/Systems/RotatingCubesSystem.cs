using System.Collections.Generic;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;
using UnityEngine;


public class RotatingCubesSystem: IUpdateSystem
{
    protected IWorldContext mWorldContext;

    public RotatingCubesSystem(IWorldContext worldContext)
    {
        mWorldContext = worldContext;
    }

    public void Update(float deltaTime)
    {
        // get all entities with TRotatingCubeComponent component
        List<uint> entities = mWorldContext.GetEntitiesWithAll(typeof(TRotatingCubeComponent));

        IEntity currEntity = null;

        for (int i = 0; i < entities.Count; ++i)
        {
            currEntity = mWorldContext.GetEntityById(entities[i]);

            // now we have two ways: strongly coupled code or use TinyECS approach
            // first version:
            /*
             * uncomment to test this version
             * 
             TViewComponent viewComponent = currEntity.GetComponent<TViewComponent>();

             TRotatingCubeComponent rotatingCubeComponent = currEntity.GetComponent<TRotatingCubeComponent>();

             viewComponent.mView.transform.Rotate(Vector3.up, rotatingCubeComponent.mSpeed); 
            */

            // second version (TinyECS approach)
            /*
             */
            TRotatingCubeComponent rotatingCubeComponent = currEntity.GetComponent<TRotatingCubeComponent>();

            TRotationComponent rotationComponent = currEntity.GetComponent<TRotationComponent>();

            currEntity.AddComponent(new TRotationComponent { mRotation = rotationComponent.mRotation * Quaternion.Euler(0.0f, rotatingCubeComponent.mSpeed, 0.0f) });
        }
    }
}