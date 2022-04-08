using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerCollision : MonoBehaviour
{
    public PlayerHandController hand;

    private LayerMask grabbableLayer;
    private LayerMask furniture;

    [HideInInspector] public bool collisionStatus;

    private void Start()
    {
        grabbableLayer = LayerMask.NameToLayer("Grabbable");
        furniture = LayerMask.NameToLayer("Furniture");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == grabbableLayer || other.gameObject.layer == furniture)
        {
            // Debug.Log(gameObject.name + " + " + other.gameObject.name + " --> Enter");
            collisionStatus = true;

            Debug.Log(this.name + " --> " + other.gameObject.name);

            // hand.PublishHandToMqtt("stop", gameObject.name);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == grabbableLayer || other.gameObject.layer == furniture)
        {
            // Debug.Log(gameObject.name + " + " + other.gameObject.name + " --> Exit");
            collisionStatus = false;

            // hand.PublishHandToMqtt("free", gameObject.name);
        }
    }
}