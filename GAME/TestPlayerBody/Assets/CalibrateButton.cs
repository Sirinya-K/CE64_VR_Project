using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrateButton : Button
{
    public MqttProtocol mqtt;
    public Image calibrateButton;
    private bool publish = false;

    void Awake()
    {
        calibrateButton.GetComponent<Image>().color = new Color32(255,73,73,255); //Red #FF4949
    }

    void FixedUpdate()
    {
        // Debug.Log(mqtt.data);

        if (mqtt.data == "nc" && !publish)
        {
            mqtt.Publish("/hr/", "s 90 90 90 90 90");
            publish = true;
        }
        else if(mqtt.data.Split(':')[0] == "hr")
        {
            calibrateButton.GetComponent<Image>().color = new Color32(21,159,56,255); //Green #159F38
            publish = false;
        }
        else if(mqtt.data == "cp")
        {
            calibrateButton.GetComponent<Image>().color = new Color32(255,137,23,255); //Orange #FF8917
            publish = false;
        }
    }
}
