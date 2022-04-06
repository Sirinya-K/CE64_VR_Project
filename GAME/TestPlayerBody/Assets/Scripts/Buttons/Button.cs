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
        pressSound = GameObject.Find("PressButton").GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(pressSound.isPlaying == false) pressSound.Play();
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Press Button");
        collision = true;
        targetObj = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        collision = false;
        targetObj = null;
    }
}
