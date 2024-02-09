using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorReset : MonoBehaviour
{
    public float maxRadius = 100f; // Maximum radius for mouse movement
    private Vector3 centerPosition; // Center of the screen

    void Start()
    {
        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0f); // Calculate the center of the screen
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center
        Cursor.visible = false; // Make cursor invisible
    }

    void Update()
    {
        // Calculate the direction vector from the center of the screen to the current mouse position
        Vector3 direction = Input.mousePosition - centerPosition;

        // If the magnitude of the direction vector exceeds the maximum radius, normalize it and scale it to the maximum radius
        if (direction.magnitude > maxRadius)
        {
            direction = direction.normalized * maxRadius;
        }

        // Calculate the new mouse position
        Vector3 newMousePosition = centerPosition + direction;

        // Convert the new mouse position from screen coordinates to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(newMousePosition);

        // Update the position of an object in the scene to simulate cursor movement
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }
}
