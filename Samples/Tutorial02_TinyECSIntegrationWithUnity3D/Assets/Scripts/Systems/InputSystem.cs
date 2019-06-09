using TinyECS.Interfaces;
using UnityEngine;


/// <summary>
/// This is our first system which processes user's input. All systems
/// which implements IUpdateSystem are executed every frame
/// </summary>

public class InputSystem: IUpdateSystem
{
    protected IWorldContext mWorldContext;

    protected IEntity       mClickInfoEntity;

    protected Camera        mMainCamera;

    public InputSystem(IWorldContext worldContext, Camera mainCamera)
    {
        mWorldContext = worldContext;

        // We create a new entity which will be unique and store information about user's clicks
        //
        // For the sake of safety methods of the framework were designed in way when they return handles not references.
        // So when you need to get some it's better to seek for the instance via its handle
        mClickInfoEntity = mWorldContext.GetEntityById(mWorldContext.CreateEntity("ClickInfoEntity"));

        mClickInfoEntity.AddComponent<TClickComponent>(/* You can also set some initial values for the component here*/);

        mMainCamera = mainCamera;
    }

    /// <summary>
    /// The method is a main part of the system and it's called every frame
    /// </summary>
    /// <param name="deltaTime"></param>

    public void Update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(mMainCamera.ScreenToWorldPoint(Input.mousePosition));

            // All later invocations of AddComponent will change the current component's value
            mClickInfoEntity.AddComponent(new TClickedComponent
            {
                mWorldPosition = mMainCamera.ScreenToWorldPoint(Input.mousePosition)
            });
        }
    }
}