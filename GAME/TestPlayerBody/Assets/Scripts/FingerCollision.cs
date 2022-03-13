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
        if (other.gameObject.layer == grabbableLayer)
        {
            // Debug.Log(gameObject.name + " + " + other.gameObject.name + " --> Enter");
            collisionStatus = true;

            hand.PublishHandToMqtt("stop", gameObject.name);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == grabbableLayer)
        {
            // Debug.Log(gameObject.name + " + " + other.gameObject.name + " --> Exit");
            collisionStatus = false;

            hand.PublishHandToMqtt("free", gameObject.name);
        }
    }
}