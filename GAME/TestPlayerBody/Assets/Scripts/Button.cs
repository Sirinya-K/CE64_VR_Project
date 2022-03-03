using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [HideInInspector]
    public bool collision;

    void OnTriggerStay(Collider other)
    {
        collision = true;
    }

    void OnTriggerExit(Collider other)
    {
        collision = false;
    }
}
