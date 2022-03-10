using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    private MqttProtocol mqtt;
    private Animator animator;

    public float RIndexValue, RMiddleValue, RThumbValue;
    public float LThumbValue, LIndexValue, LMiddleValue, LRingValue, LPinkyValue;

    private float divisor = 20f;
    private string mqttTempData = "";

    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.FindObjectOfType<MqttProtocol>();
        animator = GetComponent<Animator>();
    }

    public void PublishHandToMqtt(string fingerState, string finger)
    {
        if (fingerState == "stop")
        {
            if (finger == "R_Thumb")
            {
                mqtt.Publish("/hr/", "t" + RThumbValue * divisor);
            }
            else if (finger == "R_Index")
            {
                mqtt.Publish("/hr/", "i" + RIndexValue * divisor);
            }
            else if (finger == "R_Middle")
            {
                mqtt.Publish("/hr/", "m" + RMiddleValue * divisor);
            }
        }
        else if (fingerState == "free")
        {
            if (finger == "R_Thumb")
            {
                mqtt.Publish("/hr/", "t" + divisor);
            }
            else if (finger == "R_Index")
            {
                mqtt.Publish("/hr/", "i" + divisor);
            }
            else if (finger == "R_Middle")
            {
                mqtt.Publish("/hr/", "m" + divisor);
            }
        }
    }

    void ControlHandByMqtt()
    {
        string[] handSide = mqtt.data.Split(':');
        if (handSide[0] == "hr")
        {
            RIndexValue = float.Parse(handSide[1].Split(' ')[1]) / divisor;
            RMiddleValue = float.Parse(handSide[1].Split(' ')[2]) / divisor;
            RThumbValue = float.Parse(handSide[1].Split(' ')[0]) / divisor;
        }
    }

    void ControlHandByKeyboard()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RIndexValue -= 0.1f;
            RMiddleValue -= 0.1f;
            RThumbValue -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RIndexValue += 0.1f;
            RMiddleValue += 0.1f;
            RThumbValue += 0.1f;
        }

        // Left
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LThumbValue -= 0.1f;
            LIndexValue -= 0.1f;
            LMiddleValue -= 0.1f;
            LRingValue -= 0.1f;
            LPinkyValue -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            LThumbValue += 0.1f;
            LIndexValue += 0.1f;
            LMiddleValue += 0.1f;
            LRingValue += 0.1f;
            LPinkyValue += 0.1f;
        }
    }

    void AnimateHand()
    {
        // Right
        animator.SetFloat("R_Index", RIndexValue);
        animator.SetFloat("R_Middle", RMiddleValue);
        animator.SetFloat("R_Thumb", RThumbValue);

        // Left
        animator.SetFloat("L_Thumb", LThumbValue);
        animator.SetFloat("L_Index", LIndexValue);
        animator.SetFloat("L_Middle", LMiddleValue);
        animator.SetFloat("L_Ring", LRingValue);
        animator.SetFloat("L_Pinky", LPinkyValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (mqttTempData != mqtt.data)
        {
            // Debug.Log(mqtt.data);
            mqttTempData = mqtt.data;
            ControlHandByMqtt();
        }
        else if (Input.anyKeyDown)
        {
            ControlHandByKeyboard();
        }

        AnimateHand();
    }
}
