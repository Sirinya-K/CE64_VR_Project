using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckToggleLight : MonoBehaviour
{
    TextMesh Toggletext;

    private MqttProtocol mqtt;

    private void Awake() {
        mqtt = GameObject.FindObjectOfType<MqttProtocol> ();    
    }

    void Start()
    {
        Toggletext = GameObject.Find("TextField").GetComponent<TextMesh>();
        Debug.Log("Current Light Status: " + Toggletext.text);
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Current Light Status: " + Toggletext.text);
        if(Toggletext.text == "OFF")
        {
            mqtt.Publish("/un/out/", "ON");
            Toggletext.text = "ON";
        }
        else
        {
            mqtt.Publish("/un/out/", "OFF");
            Toggletext.text = "OFF";
        }
    }

    void Update()
    {
        
    }
}
