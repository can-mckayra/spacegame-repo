using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfinedMovement : MonoBehaviour
{
    public RectTransform crosshairTransform; // Reference to the RectTransform of the crosshair image
    public float maxDistance = 100f; // Maximum distance from the center
    private Vector2 centerPosition; // Center position of the screen

    void Start()
    {
        // Get the center position of the screen
        centerPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Update()
    {
        // Calculate the mouse movement direction
        Vector2 mousePosition = Input.mousePosition;
        Vector2 direction = (mousePosition - centerPosition).normalized;

        // Calculate the new position for the crosshair
        Vector2 newPosition = centerPosition + direction * maxDistance;

        // Ensure the crosshair stays within the circular boundary
        float distanceFromCenter = Vector2.Distance(centerPosition, newPosition);
        if (distanceFromCenter > maxDistance)
        {
            newPosition = centerPosition + direction * maxDistance;
        }

        // Apply the new position to the crosshair RectTransform
        crosshairTransform.position = newPosition;
    }
}
