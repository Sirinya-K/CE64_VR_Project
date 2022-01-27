using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [HideInInspector]
    public bool collision;

    private void OnTriggerStay(Collider other)
    {
        collision = true;
    }

    private void OnTriggerExit(Collider other)
    {
        collision = false;
    }
}
