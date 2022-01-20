using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerCollision : MonoBehaviour
{
    public PlayerHandController hand;

    private LayerMask grabbableLayer;

    [HideInInspector] public bool collisionStatus;

    private void Start()
    {
        grabbableLayer = LayerMask.NameToLayer("Grabbable");
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log(gameObject.name + ": Enter");
        collisionStatus = true;

        if (other.gameObject.layer == grabbableLayer)
        {
            hand.PublishHandToMqtt("stop", gameObject.name);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // Debug.Log(gameObject.name + ": Exit");
        collisionStatus = false;

        if (other.gameObject.layer == grabbableLayer)
        {
            hand.PublishHandToMqtt("free", gameObject.name);
        }
    }
}