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

    // Start is called before the first frame update
    void Start()
    {
        grabbableLayer = LayerMask.NameToLayer("Grabbable");

        connectedRigidbody = GetComponent<Rigidbody>();

        indexCollision = indexCollider.GetComponent<FingerCollision>();
        middleCollision = middleCollider.GetComponent<FingerCollision>();

        hand = GameObject.FindObjectOfType<PlayerHandController>();
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.layer == grabbableLayer)
        {
            if(itemJoint == null)
            {
                // Check that Index or Middle collision with the item
                if((indexCollision.collisionStatus == true && hand.RIndexValue >= 0.15) || (middleCollision.collisionStatus == true && hand.RMiddleValue >= 0.15))
                {
                    Debug.Log("Grab");

                    // Create Fixed Joint between Item and Hand
                    var itemBody = other.gameObject.GetComponent<Rigidbody>();
                    itemJoint = other.gameObject.AddComponent<FixedJoint>();
                    itemJoint.connectedBody = connectedRigidbody;

                    itemBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    itemBody.interpolation = RigidbodyInterpolation.Interpolate;
                    itemJoint.connectedMassScale = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
