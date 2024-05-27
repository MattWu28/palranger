using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 movement;

    void Update()
    {
        // Input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Normalize the input vector to ensure consistent speed in all directions
        movement = new Vector2(moveHorizontal, moveVertical).normalized;
    }

    void FixedUpdate()
    {
        // Movement
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + movement * moveSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;
    }
}
