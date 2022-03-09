using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [HideInInspector] public bool collision;
    [HideInInspector] public GameObject targetObj;

    void OnTriggerStay(Collider other)
    {
        collision = true;
        targetObj = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        collision = false;
        targetObj = null;
    }
}
