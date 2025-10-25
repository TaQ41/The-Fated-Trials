using System;
using UnityEngine;

#region Comments
// This script is a little prototype I came up with while creating a usable 3rd person camera for a player.
// As such, this code is set up, so when using it, the user can remove or add fields/parameters/function call layout.. etc.
// In other words, when using this, I wouldn't expect the code to be copied verbatim, that would just introduce avoidable inefficiencies.
// For example, If you want the user class to provide the smoothing methods, get rid of the smoothing method and just return a tuple of 2 Vector3.

// Now, this can have some use, if there is an object that needs to rotate around based on the input of a mouse, this can work very well
// and be tuned to be even faster; however, one small caveat is the fact that this will clip the rotated object through other objects given it is close enough.

// This little thing took a few days of back and forth between different possible methods (granted it only took so long due to management of school and work).
// After tidying up the position calculation.. the ratios vector looked strikingly similar to a common rotation matrix (you know, the things for
// rotating vertices in 3D.) Either way, that little section will run extremely fast, the benchmark I ran (on a good laptop) couldn't even pick up
// any milliseconds of time on the operation, a flat 0. (using the StopWatch class, I haven't tested using other benchmark methods, but a flat 0 on one is still solid)

// Final note: Don't use this for a player camera unless the player is far away from other objects or if you genuinely don't care. (or maybe it makes
// the gameplay a bit more niche?) This class will likely be using the Monobehaviour component in Unity. UnityEngine.Vector3 btw and the UnityEngine.Mathf collection
#endregion

/// <summary>
/// Find hard rotation and position of an object when provided settings, mouse input, and a smoothingMethod for the rotation.
/// Warning: This will calculate position through other objects (IE. clipping)
/// </summary>
public class ThirdPersonRotateAround
{
    /// <summary>
    /// Where in the world should the object pivot around. Local or World depends on what the class that uses this does.
    /// </summary>
    public Vector3 PivotPosition;

    /// <summary>
    /// What sensitivity should the camera have? Multiplied to the deltaMouse and Time.deltaTime for context.
    /// </summary>
    public Vector2 Sensitivity;

    /// <summary>
    /// How far the object should pivot from the object.
    /// </summary>
    public float PivotDistance;

    private float xRotation, yRotation;

    /// <summary>
    /// A sample smoothing method to help users understand how to form smoothing methods.
    /// This simply transforms the the vector3 straight to a quaternion, no filter.
    /// </summary>
    /// <param name="vec3">The required vector3 input, what the CalculateRotation method would return.</param>
    /// <returns>The quaternion required for transforms to rotate an object.</returns>
    public Quaternion DefaultSmoothingMethod(Vector3 vec3)
    {
        return Quaternion.Euler(vec3);
    }

    /// <summary>
    /// Main method that outside classes can use to get computed values for the rotation and position.
    /// </summary>
    /// <param name="deltaMouse">The change in the mouses position. This scales proportionally to the Sensitivity.</param>
    /// <param name="smoothingMethod">The smoothing method is applied to the rotation. Will essentially convert a Vector3 to a Quaternion.</param>
    /// <returns>A tuple of the Quaternion rotation and the Vector3 position</returns>
    public (Quaternion rotation, Vector3 position) CalculateRotationAndPosition(Vector2 deltaMouse, Func<Vector3, Quaternion> smoothingMethod)
    {
        Quaternion rotation = smoothingMethod(CalculateRotation(deltaMouse));
        Vector3 position = CalculatePosition(PivotPosition, PivotDistance);
        return (rotation, position);
    }

    /// <summary>
    /// Calculates rotation with the set Sensitivity and a given change in mouse position.
    /// Prevents the xRotation (looking vertically) from looking behind while only moving on that axis.
    /// </summary>
    /// <param name="deltaMouse">The change in mouse position to determine how the rotation should change.</param>
    /// <returns>A Vector3 representing the euler angles of the object's rotation.</returns>
    private Vector3 CalculateRotation(Vector2 deltaMouse)
        {
            float mouseX = deltaMouse.x * Sensitivity.x * Time.deltaTime;
            float mouseY = deltaMouse.y * Sensitivity.y * Time.deltaTime;

            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // These values should be determined elsewhere or at least be given as constants.
            return new Vector3(xRotation, yRotation, 0f);
        }

    /// <summary>
    /// Calculates position around a pivot by some distance on all axes.
    /// </summary>
    /// <param name="pivotPosition">The position in some space that determines where the object should be relatively placed (The center of the sphere).</param>
    /// <param name="distance">The distance the object should be placed away from the pivot.</param>
    /// <returns>A Vector3 representing the calculated position of the object in some space.</returns>
    private Vector3 CalculatePosition(Vector3 pivotPosition, float distance)
    {
        Vector3 ratios = new(
            Mathf.Sin(yRotation * Mathf.Deg2Rad) * Mathf.Cos(xRotation * Mathf.Deg2Rad),
            -1f * Mathf.Sin(xRotation * Mathf.Deg2Rad),
            Mathf.Cos(yRotation * Mathf.Deg2Rad) * Mathf.Cos(xRotation * Mathf.Deg2Rad)
        );

        return pivotPosition - (distance * ratios);
    }
}