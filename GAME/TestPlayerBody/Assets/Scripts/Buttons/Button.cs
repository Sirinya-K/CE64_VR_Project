using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public AudioSource pressSound;

    [HideInInspector] public bool collision;
    [HideInInspector] public GameObject targetObj;

    void Start()
    {
        pressSound = transform.Find("PressButtonSound").GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.name + " Press Button");

        if (other.gameObject.layer == LayerMask.NameToLayer("Typing Hands"))
        {
            collision = true;
            
            if (pressSound.isPlaying == false)
            {
                pressSound.Play();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        targetObj = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        collision = false;
        targetObj = null;
    }
}
