using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public Rigidbody rb;

    public float health = 100f;

    public void Hit()
    {
        health -= 45;
        if (health <= 0)
        {
            // Dead
            //Destroy(gameObject);
            rb.freezeRotation = false;
        }
    }
}
