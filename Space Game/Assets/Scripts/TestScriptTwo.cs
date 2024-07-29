using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptTwo : MonoBehaviour
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

    private void Update()
    {
        //enemyMissileObject = FindObjectOfType<MissileHandler>().gameObject;
        friendlyMissileObject = GameObject.FindWithTag("Missile");

        if (cameraRelevant != null)
        {
            Outliner(enemySpacecraftObject, enemySpacecraftOutlineUI, enemySpacecraftInRangeOutlineUI);
            //InRangeOutliner(enemySpacecraftObject, enemySpacecraftOutlineUI, enemySpacecraftInRangeOutlineUI);
            //Outliner(enemyMissileObject, enemyMissileOutlineUI);
            Outliner(friendlySpacecraftObject, friendlySpacecraftOutlineUI, null);
            Outliner(friendlyMissileObject, friendlyMissileOutlineUI, null);
        }
    }

    private void Outliner(GameObject _gameObject, RectTransform _objectOutlineUI, RectTransform _objectInRangeOutlineUI)
    {
        if (_gameObject != null && _objectOutlineUI != null)
        {
            Vector3 _objectScreenPosition = cameraRelevant.WorldToScreenPoint(_gameObject.transform.position);

            if (_objectScreenPosition.z < 0)
            {
                _objectOutlineUI.gameObject.SetActive(false);
            }
            else if (_objectScreenPosition.z >= 0)
            {
                _objectOutlineUI.gameObject.SetActive(true);
            }

            if (radarCheck.targetLocked == true)
            {
                if (_gameObject != null && _objectInRangeOutlineUI != null)
                {
                    if (_objectScreenPosition.z < 0)
                    {
                        _objectInRangeOutlineUI.gameObject.SetActive(false);
                    }
                    else if (_objectScreenPosition.z >= 0)
                    {
                        _objectInRangeOutlineUI.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (_objectInRangeOutlineUI != null)
                {
                    _objectInRangeOutlineUI.gameObject.SetActive(false);
                }
            }

            if (_objectScreenPosition.z < 0 || _objectScreenPosition.z > 998 || _objectScreenPosition.x < 0 || _objectScreenPosition.x > Screen.width || _objectScreenPosition.y < 0 || _objectScreenPosition.y > Screen.height)
            {
                _objectScreenPosition.x = Mathf.Clamp(_objectScreenPosition.x, 0, Screen.width);
                _objectScreenPosition.y = Mathf.Clamp(_objectScreenPosition.y, 0, Screen.height);
                _objectScreenPosition.z = Mathf.Clamp(_objectScreenPosition.z, 0, 999);
            }

            _objectOutlineUI.position = _objectScreenPosition;

            if (_objectInRangeOutlineUI != null)
            {
                _objectInRangeOutlineUI.position = _objectScreenPosition;
            }
        }
        if (_gameObject == null)
        {
            _objectOutlineUI.gameObject.SetActive(false);
        }
    }
    /*
    private void InRangeOutliner(GameObject _gameObject, RectTransform _objectOutlineUI, RectTransform _objectInRangeOutlineUI)
    {
        if (radarCheck.targetLocked == true)
        {
            if (_gameObject != null && _objectInRangeOutlineUI != null)
            {
                Vector3 _objectScreenPosition = cameraRelevant.WorldToScreenPoint(_gameObject.transform.position);

                if (_objectScreenPosition.z < 0)
                {
                    _objectInRangeOutlineUI.gameObject.SetActive(false);
                }
                else if (_objectScreenPosition.z >= 0)
                {
                    _objectInRangeOutlineUI.gameObject.SetActive(true);
                }

                if (_objectScreenPosition.z < 0 || _objectScreenPosition.z > 998 || _objectScreenPosition.x < 0 || _objectScreenPosition.x > Screen.width || _objectScreenPosition.y < 0 || _objectScreenPosition.y > Screen.height)
                {
                    _objectScreenPosition.x = Mathf.Clamp(_objectScreenPosition.x, 0, Screen.width);
                    _objectScreenPosition.y = Mathf.Clamp(_objectScreenPosition.y, 0, Screen.height);
                    _objectScreenPosition.z = Mathf.Clamp(_objectScreenPosition.z, 0, 999);
                }

                _objectInRangeOutlineUI.position = _objectScreenPosition;
            }
        }
    }
    */
}
