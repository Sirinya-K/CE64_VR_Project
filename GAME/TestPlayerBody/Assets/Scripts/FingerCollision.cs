using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerCollision : MonoBehaviour
{
    [HideInInspector] public bool collisionStatus;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(gameObject.name + ": Enter");
        collisionStatus = true;
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log(gameObject.name + ": Exit");
        collisionStatus = false;
    }
}
