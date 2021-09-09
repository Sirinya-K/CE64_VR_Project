using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    private Rigidbody handRigidbody;
    private Rigidbody itemRigidbody;

    private FixedJoint itemJoint, handJoint;
    private LayerMask grabbableLayer;

    private MqttProtocol mqtt;
    private CheckFingerCollision fingerCollider;

    private void Awake() {
        mqtt = GameObject.FindObjectOfType<MqttProtocol>();    
        fingerCollider = GameObject.FindObjectOfType<CheckFingerCollision>();
    }

    void Start()
    {
        handRigidbody = GetComponent<Rigidbody>();
        grabbableLayer = LayerMask.NameToLayer("Grabbable");
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Object: " + gameObject.name + ", Collision with: " + other.collider.name);
        Debug.Log("Grab");

        if(other.gameObject.layer == grabbableLayer && itemJoint == null)
        {
            // Debug.Log("Palm & Finger Collision !!!");

            // itemJoint = other.gameObject.AddComponent<FixedJoint>();
            // itemJoint.connectedBody = handRigidbody;

            // itemJoint.anchor = Vector3.zero;
            // itemJoint.axis = Vector3.zero;
            // itemJoint.autoConfigureConnectedAnchor = false;
            // itemJoint.connectedAnchor = new Vector3 (other.transform.localScale.x * 20, 0, 0);
            // itemJoint.connectedAnchor = other.transform.localScale;
            // itemJoint.secondaryAxis = Vector3.zero;

            // itemJoint.xMotion = ConfigurableJointMotion.Locked;
            // itemJoint.yMotion = ConfigurableJointMotion.Locked;
            // itemJoint.zMotion = ConfigurableJointMotion.Locked;
            // itemJoint.angularXMotion = ConfigurableJointMotion.Locked;
            // itemJoint.angularYMotion = ConfigurableJointMotion.Locked;
            // itemJoint.angularZMotion = ConfigurableJointMotion.Locked;

            var itemBody = other.gameObject.GetComponent<Rigidbody>();
            
            itemJoint = other.gameObject.AddComponent<FixedJoint>();
            itemJoint.connectedBody = handRigidbody;

            handJoint = gameObject.AddComponent<FixedJoint>();
            handJoint.connectedBody = itemBody;

            itemBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            itemBody.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
    }

    private void Release()
    {
        Debug.Log("Release");
        Destroy(itemJoint); 
        Destroy(handJoint); 
    }

    void Update()
    {
        if(mqtt.data == "drop")
        {
            Debug.Log(mqtt.data);
            Release();
            mqtt.clear_data();
        }

        // Debug.Log("Finger: " + fingerCollider.collisionStatus);

        // if(itemJoint != null && fingerCollider.collisionStatus == false)
        // {
        //     Debug.Log("Release();");
        //     Release();
        // }
    }
}
