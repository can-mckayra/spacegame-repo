using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int teamID = 1;
    public int playerID = 1;

    public float maxHealth = 500f;
    public float currentHealth;

    void Start()
    {
        gameObject.layer = teamID + 10;
        currentHealth = maxHealth;
    }
}
