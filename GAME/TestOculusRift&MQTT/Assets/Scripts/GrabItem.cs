using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    private Rigidbody handRigidbody;
    private ConfigurableJoint itemJoint;
    private LayerMask grabbableLayer;

    private MqttProtocol mqtt;

    private void Awake() {
        mqtt = GameObject.FindObjectOfType<MqttProtocol> ();    
    }

    void Start()
    {
        handRigidbody = GetComponent<Rigidbody>();
        grabbableLayer = LayerMask.NameToLayer("Grabbable");
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == grabbableLayer && itemJoint == null)
        {
            Debug.Log("object: " + gameObject.name + ", Collision with: " + other.collider.name);
            itemJoint = other.gameObject.AddComponent<ConfigurableJoint>();
            itemJoint.connectedBody = handRigidbody;

            itemJoint.anchor = new Vector3 (0, 0, 0);
            itemJoint.axis = new Vector3 (0, 0, 0);
            itemJoint.autoConfigureConnectedAnchor = false;
            itemJoint.connectedAnchor = new Vector3 (other.transform.localScale.x * 20, 0, 0);
            itemJoint.secondaryAxis = new Vector3 (0, 0, 0);

            itemJoint.xMotion = ConfigurableJointMotion.Locked;
            itemJoint.yMotion = ConfigurableJointMotion.Locked;
            itemJoint.zMotion = ConfigurableJointMotion.Locked;
            itemJoint.angularXMotion = ConfigurableJointMotion.Locked;
            itemJoint.angularYMotion = ConfigurableJointMotion.Locked;
            itemJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }
    }

    private void Release()
    {
        if(itemJoint != null)
        {
            Destroy(itemJoint);
        }
    }

    void Update()
    {
        if(mqtt.data == "drop")
        {
            Debug.Log(mqtt.data);
            Release();
            mqtt.clear_data();
        }
    }
}
