using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smooth speed for camera movement
    public Vector3 offset; // Offset between the player and camera

    private void LateUpdate()
    {
        // Calculate the desired position of the camera based on player's position and the offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly move towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
