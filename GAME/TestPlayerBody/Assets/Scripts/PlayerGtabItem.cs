using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGtabItem : MonoBehaviour
{
    private LayerMask grabbableLayer;

    private Rigidbody connectedRigidbody;
    private FixedJoint itemJoint;

    [SerializeField] private GameObject indexCollider;
    [SerializeField] private GameObject middleCollider;

    private FingerCollision indexCollision, middleCollision;

    private PlayerHandController hand;

    [SerializeField] private GameObject topPalm;
    [SerializeField] private GameObject bottomPalm;
    private Rigidbody topPalmRigidbody, bottomPalmRigidbody;
    private FixedJoint itemJoint1, itemJoint2;

    // Start is called before the first frame update
    void Start()
    {
        grabbableLayer = LayerMask.NameToLayer("Grabbable");

        connectedRigidbody = GetComponent<Rigidbody>();

        indexCollision = indexCollider.GetComponent<FingerCollision>();
        middleCollision = middleCollider.GetComponent<FingerCollision>();

        hand = GameObject.FindObjectOfType<PlayerHandController>();

        topPalmRigidbody = topPalm.GetComponent<Rigidbody>();
        bottomPalmRigidbody = bottomPalm.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.layer == grabbableLayer)
        {
            if(other.gameObject.GetComponent<FixedJoint>() == null)
            {
                // Check that Index or Middle collision with the item
                if((indexCollision.collisionStatus == true && hand.RIndexValue >= 0.15) || (middleCollision.collisionStatus == true && hand.RMiddleValue >= 0.15))
                {
                    Debug.Log("Grab");

                    // Create Fixed Joint between the Item and Hand
                    var itemBody = other.gameObject.GetComponent<Rigidbody>();
                    itemBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    itemBody.interpolation = RigidbodyInterpolation.Interpolate;
                    
                    itemJoint = other.gameObject.AddComponent<FixedJoint>();
                    itemJoint.connectedBody = connectedRigidbody;
                    itemJoint.connectedMassScale = 0;

                    // Set the Item's gravity to false
                    itemBody.useGravity = false;

                    // Add CheckVelocity Script to thr Item
                    other.gameObject.AddComponent<CheckVelocity>();

                    // itemJoint1 = other.gameObject.AddComponent<FixedJoint>();
                    // itemJoint1.connectedBody = topPalmRigidbody;
                    // itemJoint1.connectedMassScale = 0;

                    // itemJoint2 = other.gameObject.AddComponent<FixedJoint>();
                    // itemJoint2.connectedBody = bottomPalmRigidbody;
                    // itemJoint2.connectedMassScale = 0;
                }   
            }
        }
    }

    private void OnCollisionExit(Collision other) {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
