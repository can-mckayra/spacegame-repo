using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyGameObject;
    [SerializeField] private bool inRadar = false;
    [SerializeField] private Camera relevantCamera;
    [SerializeField] private Vector2 screenCenter;
    [SerializeField] private float radius = 25.0f;
    [SerializeField] private Vector2 targetScreenCoords;

    private void Start()
    {
        Debug.Log(radius);
        screenCenter = new(Screen.width / 2.0f, Screen.height / 2.0f);
    }

    private void Update()
    {
        InRadarCheck();
        //Debug.Log(relevantCamera.WorldToScreenPoint(enemyGameObject.transform.position).x);
        //Debug.Log((new Vector2(relevantCamera.WorldToScreenPoint(enemyGameObject.transform.position).x, relevantCamera.WorldToScreenPoint(enemyGameObject.transform.position).y) - screenCenter).magnitude);
        //Debug.Log((targetScreenCoords - screenCenter).magnitude);
    }

    private void InRadarCheck()
    {
        targetScreenCoords = new(relevantCamera.WorldToScreenPoint(enemyGameObject.transform.position).x, relevantCamera.WorldToScreenPoint(enemyGameObject.transform.position).y);

        if ((targetScreenCoords - screenCenter).magnitude < radius)
        {
            //Debug.Log("inRadar");
            inRadar = true;
        }
        else if ((targetScreenCoords - screenCenter).magnitude > radius)
        {
            //Debug.Log("not inRadar");
            inRadar = false;
        }
    }
}