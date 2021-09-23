using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    private Rigidbody handRigidbody;

    private FixedJoint itemJoint, handJoint;
    private LayerMask grabbableLayer;

    private MqttProtocol mqtt;

    public bool finCollisionStatus = false;

    private int buff;

    private GameObject temp;
    private Rigidbody tempRigidbody;
    private FixedJoint tempJoint;

    private void Awake() {
        mqtt = GameObject.FindObjectOfType<MqttProtocol>();    
    }

    void Start()
    {
        handRigidbody = GetComponent<Rigidbody>();
        grabbableLayer = LayerMask.NameToLayer("Grabbable");
        // temp = GameObject.Find("Temp Collider");
        // tempRigidbody = temp.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision other)
    {
        // Debug.Log("Object: " + gameObject.name + ", Collision with: " + other.collider.name);
        // Debug.Log("OnCollisionStay()");

        // if(other.gameObject.layer == grabbableLayer && itemJoint == null)
        if(other.gameObject.layer == grabbableLayer && itemJoint == null && finCollisionStatus == true && buff > 1)
        {
            Debug.Log("Grab");

            // ----- Configurable Joint -----
            // itemJoint = other.gameObject.AddComponent<ConfigurableJoint>();
            // itemJoint.connectedBody = handRigidbody;

            // itemJoint.autoConfigureConnectedAnchor = false;
            // itemJoint.connectedAnchor = new Vector3 (other.transform.localScale.x * 20, 0, 0);
            // itemJoint.connectedAnchor = itemJoint.connectedAnchor * multiplier;
            // itemJoint.autoConfigureConnectedAnchor = false;
            // itemJoint.secondaryAxis = Vector3.zero;

            // itemJoint.xMotion = ConfigurableJointMotion.Locked;
            // itemJoint.yMotion = ConfigurableJointMotion.Locked;
            // itemJoint.zMotion = ConfigurableJointMotion.Locked;
            // itemJoint.angularXMotion = ConfigurableJointMotion.Locked;
            // itemJoint.angularYMotion = ConfigurableJointMotion.Locked;
            // itemJoint.angularZMotion = ConfigurableJointMotion.Locked;

            // ----- Fixed Joint -----
            var itemBody = other.gameObject.GetComponent<Rigidbody>();
            
            itemJoint = other.gameObject.AddComponent<FixedJoint>();
            itemJoint.connectedBody = handRigidbody;

            // handJoint = gameObject.AddComponent<FixedJoint>();
            // handJoint.connectedBody = itemBody;

            // itemJoint = other.gameObject.AddComponent<FixedJoint>();
            // itemJoint.connectedBody = tempRigidbody;

            // tempJoint = temp.AddComponent<FixedJoint>();
            // tempJoint.connectedBody = itemBody;

            itemBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            itemBody.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
    }

    private void Release()
    {
        Debug.Log("Release");
        Destroy(itemJoint); 
        // Destroy(handJoint); 
    }

    void Update()
    {
        // if(mqtt.data == "drop")
        // {
        //     Debug.Log(mqtt.data);
        //     Release();
        //     mqtt.clear_data();
        // }
        
        if (mqtt.data!="")
        {
            string[] buffer = mqtt.data.Split(':');
            if(buffer[0] == "hl" || buffer[0] == "hr")
            {
                buff=int.Parse(buffer[1].Split(' ')[1]);
            }
        }
        // Debug.Log("buff: " + buff);

        // if((itemJoint != null && finCollisionStatus == false) || (itemJoint != null && buff <= 1))
        // if(itemJoint != null && buff <= 1)
        if((itemJoint != null && finCollisionStatus == false) || (itemJoint != null && buff <= 1))
        {
            Release();
        }
    }
}
