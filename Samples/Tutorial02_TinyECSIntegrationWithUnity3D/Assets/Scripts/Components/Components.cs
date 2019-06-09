using TinyECS.Interfaces;
using UnityEngine;

/// <summary>
/// This type defines a flag-component, because it doesn't store
/// some internal data
/// </summary>

public struct TClickComponent : IComponent
{
}

/// <summary>
/// This type contains an information about user's input. You can store
/// any type of information you want.
/// </summary>

public struct TClickedComponent : IComponent
{
    public Vector2 mWorldPosition;
}


public struct TRotatingCubeComponent: IComponent
{
    public float mSpeed;
}


public struct TRotationComponent: IComponent
{
    public Quaternion mRotation;
}