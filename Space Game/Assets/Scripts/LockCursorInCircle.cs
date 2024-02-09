using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursorInCircle : MonoBehaviour
{
    public float radius = 50f;
    public Texture2D crosshairTexture;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void OnGUI()
    {
        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mousePos = Event.current.mousePosition;

        // Calculate the distance between the mouse position and the center of the circle
        float distance = Vector2.Distance(mousePos, center);

        // If the distance is greater than the radius, clamp the mouse position to the edge of the circle
        if (distance > radius)
        {
            mousePos = center + (mousePos - center).normalized * radius;
            //Debug.Log(mousePos);
        }

        GUI.DrawTexture(new Rect(mousePos.x - (crosshairTexture.width / 2), mousePos.y - (crosshairTexture.height / 2), crosshairTexture.width, crosshairTexture.height), crosshairTexture);
    }
}
