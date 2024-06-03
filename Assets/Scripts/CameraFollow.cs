using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform component
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Offset to maintain between camera and player

    void LateUpdate()
    {
        if (player != null)
        {
            // Update the camera's position to follow the player's position with an offset
            transform.position = player.position + offset;
        }
    }
}