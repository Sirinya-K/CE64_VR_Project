using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingArea : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftTypingHand;
    public GameObject rightTypingHand;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "L_Index")
        {
            leftTypingHand.SetActive(true);
        }
        else if(other.gameObject.name == "R_Index")
        {
            rightTypingHand.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "L_Index")
        {
            leftTypingHand.SetActive(false);
        }
        else if(other.gameObject.name == "R_Index")
        {
            rightTypingHand.SetActive(false);
        }
    }
}
