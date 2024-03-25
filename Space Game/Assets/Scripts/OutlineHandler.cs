using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOutliner : MonoBehaviour
{
    [SerializeField] private RadarCheck radarCheck;

    public GameObject enemySpacecraftObject;
    public RectTransform enemySpacecraftOutlineUI;
    public RectTransform enemySpacecraftInRangeOutlineUI;
    public GameObject enemyMissileObject;
    public RectTransform enemyMissileOutlineUI;

    public GameObject friendlySpacecraftObject;
    public RectTransform friendlySpacecraftOutlineUI;
    public GameObject friendlyMissileObject;
    public RectTransform friendlyMissileOutlineUI;

    public Camera cameraRelevant;

    private void Start()
    {
        friendlyMissileOutlineUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        //enemyMissileObject = FindObjectOfType<MissileHandler>().gameObject;
        friendlyMissileObject = GameObject.FindWithTag("Missile");

        if (cameraRelevant != null)
        {
            EnemySpacecraftOutliner();
            EnemySpacecraftInRangeOutliner();
            //EnemyMissileOutliner();
            FriendlySpaceCraftOutliner();
            FriendlyMissileOutliner();
        }
    }

    private void EnemySpacecraftOutliner()
    {
        if (enemySpacecraftObject != null && enemySpacecraftOutlineUI != null)
        {
            enemySpacecraftInRangeOutlineUI.gameObject.SetActive(false);

            Vector3 enemySpacecraftScreenPosition = cameraRelevant.WorldToScreenPoint(enemySpacecraftObject.transform.position);

            if (enemySpacecraftScreenPosition.z < 0)
            {
                enemySpacecraftOutlineUI.gameObject.SetActive(false);
            }
            else if (enemySpacecraftScreenPosition.z >= 0)
            {
                enemySpacecraftOutlineUI.gameObject.SetActive(true);
            }

            if (enemySpacecraftScreenPosition.z < 0 || enemySpacecraftScreenPosition.z > 998 || enemySpacecraftScreenPosition.x < 0 || enemySpacecraftScreenPosition.x > Screen.width || enemySpacecraftScreenPosition.y < 0 || enemySpacecraftScreenPosition.y > Screen.height)
            {
                // If off-screen, clamp the screen position to within the screen bounds
                enemySpacecraftScreenPosition.x = Mathf.Clamp(enemySpacecraftScreenPosition.x, 0, Screen.width);
                enemySpacecraftScreenPosition.y = Mathf.Clamp(enemySpacecraftScreenPosition.y, 0, Screen.height);
                enemySpacecraftScreenPosition.z = Mathf.Clamp(enemySpacecraftScreenPosition.y, 0, 999);
            }

            enemySpacecraftOutlineUI.position = enemySpacecraftScreenPosition;
        }
    }

    private void EnemySpacecraftInRangeOutliner()
    {
        if (radarCheck.targetLocked == true)
        {
            if (enemySpacecraftObject != null && enemySpacecraftInRangeOutlineUI != null && enemySpacecraftOutlineUI != null)
            {
                Vector3 enemySpacecraftScreenPosition = cameraRelevant.WorldToScreenPoint(enemySpacecraftObject.transform.position);

                if (enemySpacecraftScreenPosition.z < 0)
                {
                    enemySpacecraftInRangeOutlineUI.gameObject.SetActive(false);
                }
                else if (enemySpacecraftScreenPosition.z >= 0)
                {
                    enemySpacecraftInRangeOutlineUI.gameObject.SetActive(true);
                }

                if (enemySpacecraftScreenPosition.z < 0 || enemySpacecraftScreenPosition.z > 998 || enemySpacecraftScreenPosition.x < 0 || enemySpacecraftScreenPosition.x > Screen.width || enemySpacecraftScreenPosition.y < 0 || enemySpacecraftScreenPosition.y > Screen.height)
                {
                    // If off-screen, clamp the screen position to within the screen bounds
                    enemySpacecraftScreenPosition.x = Mathf.Clamp(enemySpacecraftScreenPosition.x, 0, Screen.width);
                    enemySpacecraftScreenPosition.y = Mathf.Clamp(enemySpacecraftScreenPosition.y, 0, Screen.height);
                    enemySpacecraftScreenPosition.z = Mathf.Clamp(enemySpacecraftScreenPosition.y, 0, 999);
                }

                enemySpacecraftInRangeOutlineUI.position = enemySpacecraftScreenPosition;
            }
        }
        else
        {
            enemySpacecraftInRangeOutlineUI.gameObject.SetActive(false);
        }
    }

    private void EnemyMissileOutliner()
    {
        if (enemyMissileObject != null)
        {
            friendlyMissileOutlineUI.gameObject.SetActive(true);

            Vector3 enemyMissileScreenPosition = cameraRelevant.WorldToScreenPoint(enemyMissileObject.transform.position);

            if (enemyMissileScreenPosition.z < 0)
            {
                enemyMissileOutlineUI.gameObject.SetActive(false);
            }
            else if (enemyMissileScreenPosition.z >= 0)
            {
                enemyMissileObject.gameObject.SetActive(true);
            }

            if (enemyMissileScreenPosition.z < 0 || enemyMissileScreenPosition.z > 998 || enemyMissileScreenPosition.x < 0 || enemyMissileScreenPosition.x > Screen.width || enemyMissileScreenPosition.y < 0 || enemyMissileScreenPosition.y > Screen.height)
            {
                // If off-screen, clamp the screen position to within the screen bounds
                enemyMissileScreenPosition.x = Mathf.Clamp(enemyMissileScreenPosition.x, 0, Screen.width);
                enemyMissileScreenPosition.y = Mathf.Clamp(enemyMissileScreenPosition.y, 0, Screen.height);
                enemyMissileScreenPosition.z = Mathf.Clamp(enemyMissileScreenPosition.y, 0, 999);
            }

            enemyMissileOutlineUI.position = enemyMissileScreenPosition;
        }
        else
        {
            enemyMissileOutlineUI.gameObject.SetActive(false);
        }
    }

    private void FriendlySpaceCraftOutliner()
    {
        if (friendlySpacecraftObject != null && friendlySpacecraftOutlineUI != null)
        {
            Vector3 friendlySpacecraftScreenPosition = cameraRelevant.WorldToScreenPoint(friendlySpacecraftObject.transform.position);

            if (friendlySpacecraftScreenPosition.z < 0)
            {
                friendlySpacecraftOutlineUI.gameObject.SetActive(false);
            }
            else if (friendlySpacecraftScreenPosition.z >= 0)
            {
                friendlySpacecraftOutlineUI.gameObject.SetActive(true);
            }

            if (friendlySpacecraftScreenPosition.z < 0 || friendlySpacecraftScreenPosition.z > 998 || friendlySpacecraftScreenPosition.x < 0 || friendlySpacecraftScreenPosition.x > Screen.width || friendlySpacecraftScreenPosition.y < 0 || friendlySpacecraftScreenPosition.y > Screen.height)
            {
                // If off-screen, clamp the screen position to within the screen bounds
                friendlySpacecraftScreenPosition.x = Mathf.Clamp(friendlySpacecraftScreenPosition.x, 0, Screen.width);
                friendlySpacecraftScreenPosition.y = Mathf.Clamp(friendlySpacecraftScreenPosition.y, 0, Screen.height);
                friendlySpacecraftScreenPosition.z = Mathf.Clamp(friendlySpacecraftScreenPosition.y, 0, 999);
            }

            friendlySpacecraftOutlineUI.position = friendlySpacecraftScreenPosition;
            //Debug.Log(friendlySpacecraftScreenPosition);
        }
    }

    private void FriendlyMissileOutliner()
    {
        if (friendlyMissileObject != null)
        {
            friendlyMissileOutlineUI.gameObject.SetActive(true);

            Vector3 friendlyMissileScreenPosition = cameraRelevant.WorldToScreenPoint(friendlyMissileObject.transform.position);

            if (friendlyMissileScreenPosition.z < 0)
            {
                friendlyMissileOutlineUI.gameObject.SetActive(false);
            }
            else if (friendlyMissileScreenPosition.z >= 0)
            {
                friendlyMissileObject.gameObject.SetActive(true);
            }

            if (friendlyMissileScreenPosition.z < 0 || friendlyMissileScreenPosition.z > 998 || friendlyMissileScreenPosition.x < 0 || friendlyMissileScreenPosition.x > Screen.width || friendlyMissileScreenPosition.y < 0 || friendlyMissileScreenPosition.y > Screen.height)
            {
                // If off-screen, clamp the screen position to within the screen bounds
                friendlyMissileScreenPosition.x = Mathf.Clamp(friendlyMissileScreenPosition.x, 0, Screen.width);
                friendlyMissileScreenPosition.y = Mathf.Clamp(friendlyMissileScreenPosition.y, 0, Screen.height);
                friendlyMissileScreenPosition.z = Mathf.Clamp(friendlyMissileScreenPosition.y, 0, 999);
            }

            friendlyMissileOutlineUI.position = friendlyMissileScreenPosition;
        }
        else
        {
            friendlyMissileOutlineUI.gameObject.SetActive(false);
        }
    }
}
