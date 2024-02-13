using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ConfinedMovement : MonoBehaviour
{
    public RectTransform crosshair;
    public float crosshairMultiplier = 10.0f;
    public float normalizeMagnitude = 100.0f;
    public float radius = 10000.0f;
    public float speed = 50.0f;

    public Vector3 screenCenter;
    public Vector3 crosshairOrigin;

    void Start()
    {
        Cursor.visible = false;
        screenCenter = new(Screen.width / 2f, Screen.height / 2f, 0.0f);
        crosshairOrigin = screenCenter;
    }

    void Update()
    {
        // Get mouse input
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // Add input to crosshair
        crosshair.position += new Vector3(x * crosshairMultiplier, y * crosshairMultiplier, 0.0f);

        // Move crosshair to origin to apply circular clamp
        crosshairOrigin = crosshair.position - screenCenter;

        // Clamp vector magnitude to 10
        if (crosshairOrigin.sqrMagnitude > radius)
        {
            crosshairOrigin.Normalize();
            crosshairOrigin *= normalizeMagnitude;
        }

        // Update crosshair position
        crosshair.position = crosshairOrigin + screenCenter;

        Debug.Log(crosshairOrigin);
        //Debug.Log(Input.mousePosition.x + ", " + Input.mousePosition.y);
    }
}