using UnityEngine;

namespace Movement
{

/// <summary>
/// 
/// </summary>
public class FlatPlayerMovement : MonoBehaviour
{
    // Assumes that this will be attached to the player object in scene.

    [SerializeField]
    private new Rigidbody rigidbody;
    // Start with a simple method that moves a player's rigidbody forward with a set force.

    public void MovePlayerByOrientation(Vector2 dir)
    {
        Debug.Log($"Rigidbody: {rigidbody}");
        Debug.Log($"Direction: {dir}");
        return;
    }
}
}