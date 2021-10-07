using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFingerCollision : MonoBehaviour
{
    private GrabItem grabItem;
    // public bool collisionStatus = false;

    private void Awake()
    {
        grabItem = GameObject.FindObjectOfType<GrabItem>();
    }
    private void OnCollisionEnter(Collision other)
    {
        grabItem.finCollisionStatus = true;
        Debug.Log("Object: " + gameObject.name + ", Collision with: " + other.collider.name + ", Status: " + grabItem.finCollisionStatus);
    }

    private void OnCollisionExit(Collision other)
    {
        grabItem.finCollisionStatus = false;
        Debug.Log("Object: " + gameObject.name + ", Collision with: " + other.collider.name + ", Status: " + grabItem.finCollisionStatus);
    }
}
