using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    private Vector3 previousMousePosition;

    void Update()
    {
        // Store current mouse position
        Vector3 currentMousePosition = Input.mousePosition;

        // Compare current mouse position with previous frame's mouse position
        if (currentMousePosition != previousMousePosition)
        {
            // Mouse position has changed
            Debug.Log("Mouse position has changed from " + previousMousePosition + " to " + currentMousePosition);
        }

        // Update previous mouse position for next frame
        previousMousePosition = currentMousePosition;
    }
}
