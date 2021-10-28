using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera theCamera;

    void FixedUpdate()
    {
        transform.LookAt(2*transform.position - theCamera.transform.position);
    }
}
