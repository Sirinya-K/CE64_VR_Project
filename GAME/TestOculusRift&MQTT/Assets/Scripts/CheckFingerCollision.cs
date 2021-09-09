using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFingerCollision : MonoBehaviour
{
    public bool collisionStatus = false;

    private void OnCollisionEnter(Collision other)
    {
        collisionStatus = true;
        Debug.Log("Object: " + gameObject.name + ", Collision with: " + other.collider.name + ", Status: " + collisionStatus);
    }

    private void OnCollisionExit(Collision other)
    {
        collisionStatus = false;
        Debug.Log("Object: " + gameObject.name + ", Collision with: " + other.collider.name + ", Status: " + collisionStatus);
    }
}
