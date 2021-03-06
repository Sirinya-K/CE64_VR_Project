using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEntrance : MonoBehaviour
{
    [HideInInspector]
    public bool collision;

    private void OnTriggerEnter(Collider other)
    {
        collision = true;
    }

    private void OnTriggerExit(Collider other)
    {
        collision = false;
    }
}
