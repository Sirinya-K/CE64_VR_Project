using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVelocity : MonoBehaviour
{
    private Rigidbody itemRigidbody;
    private float itemVelocityMagnitude;

    // Start is called before the first frame update
    void Start()
    {
        itemRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        itemVelocityMagnitude = itemRigidbody.velocity.magnitude;
        // Debug.Log("Velocity Magnitude of " + gameObject.name + ": " + itemVelocityMagnitude);
    }
}
