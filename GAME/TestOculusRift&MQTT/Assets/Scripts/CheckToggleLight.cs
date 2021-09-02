using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLightScript : MonoBehaviour
{
    TextMesh Toggletext;

    void Start()
    {
        Toggletext = GameObject.Find("TextField").GetComponent<TextMesh>();
    }

    void Update()
    {
        
    }
}
