using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGtabItem : MonoBehaviour
{
    private LayerMask grabbableLayer;

    private Rigidbody connectedBody, itemBody;

    private FixedJoint itemJoint;

    [SerializeField]
    private GameObject indexCollider;

    [SerializeField]
    private GameObject middleCollider;

    private FingerCollision indexCollision, middleCollision;

    private PlayerHandController hand;

    private string finger = "";
    private float fingerValue = 0f;
    private float diff = 0.1f;

    // private Collider[] itemColliders;

    // Start is called before the first frame update
    void Start()
    {
        grabbableLayer = LayerMask.NameToLayer("Grabbable");

        connectedBody = GetComponent<Rigidbody>();

        indexCollision = indexCollider.GetComponent<FingerCollision>();
        middleCollision = middleCollider.GetComponent<FingerCollision>();

        hand = GameObject.FindObjectOfType<PlayerHandController>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == grabbableLayer)
        {
            if (other.gameObject.GetComponent<FixedJoint>() == null)
            {
                // Check that Index or Middle collision with the item
                if ((indexCollision.collisionStatus == true && hand.RIndexValue >= 0.15) || (middleCollision.collisionStatus == true && hand.RMiddleValue >= 0.15))
                {
                    Debug.Log("Grab");

                    // Create Fixed Joint between the Item and Hand
                    itemJoint = other.gameObject.AddComponent<FixedJoint>();
                    itemJoint.connectedBody = connectedBody;
                    itemJoint.connectedMassScale = 0;
                    
                    itemBody = other.gameObject.GetComponent<Rigidbody>();
                    itemBody.useGravity = false;
                    itemBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    itemBody.interpolation = RigidbodyInterpolation.Interpolate;

                    // Add CheckVelocity Script to the Item
                    other.gameObject.AddComponent<CheckVelocity>();

                    // Save Finger Value
                    if (indexCollision.collisionStatus && middleCollision.collisionStatus)
                    {
                        finger = "both";
                        fingerValue = hand.RMiddleValue;
                    }
                    else if (indexCollision.collisionStatus)
                    {
                        finger = "index";
                        fingerValue = hand.RIndexValue;
                    }
                    else if (middleCollision.collisionStatus)
                    {
                        finger = "middle";
                        fingerValue = hand.RMiddleValue;
                    }
                    
                    // Set Trigger on the Item
                    // itemColliders = other.gameObject.GetComponentsInChildren<Collider>();
                    // foreach (Collider c in itemColliders) c.isTrigger = true;
                }
            }
        }
    }

    private void Release()
    {
        Debug.Log("Release");

        Destroy(itemJoint);
        itemBody.useGravity = true;
        itemBody = null;

        // foreach (Collider c in itemColliders) c.isTrigger = false;

        finger = "";
        fingerValue = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (finger == "index" && (fingerValue - hand.RIndexValue >= diff))
        {
            Release();
        }
        else if (finger == "middle" && (fingerValue - hand.RMiddleValue >= diff))
        {
            Release();
        }
        else if (finger == "both" && (fingerValue - hand.RIndexValue >= diff && fingerValue - hand.RMiddleValue >= diff))
        {
            Release();
        }
    }
}
