using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    private MqttProtocol mqtt;
    private Animator animator;

    public float RThumbValue, RIndexValue, RMiddleValue, RRingValue, RPinkyValue;
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
            else if (finger == "R_Ring")
            {
                mqtt.Publish("/hr/", "r" + RRingValue * divisor);
            }
            else if (finger == "R_Pinky")
            {
                mqtt.Publish("/hr/", "p" + RPinkyValue * divisor);
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
            else if (finger == "R_Ring")
            {
                mqtt.Publish("/hr/", "r" + divisor);
            }
            else if (finger == "R_Pinky")
            {
                mqtt.Publish("/hr/", "p" + divisor);
            }
        }
    }

    void ControlHandByMqtt()
    {
        string[] handSide = mqtt.data.Split(':');

        // Right
        if (handSide[0] == "hr")
        {
            RThumbValue = float.Parse(handSide[1].Split(' ')[0]) / divisor;
            RIndexValue = float.Parse(handSide[1].Split(' ')[1]) / divisor;
            RMiddleValue = float.Parse(handSide[1].Split(' ')[2]) / divisor;
            RRingValue = float.Parse(handSide[1].Split(' ')[3]) / divisor;
            RPinkyValue = float.Parse(handSide[1].Split(' ')[4]) / divisor;
        }

        // Left
        // if (handSide[0] == "hl")
        // {
        //     LThumbValue = float.Parse(handSide[1].Split(' ')[0]) / divisor;
        //     LIndexValue = float.Parse(handSide[1].Split(' ')[1]) / divisor;
        //     LMiddleValue = float.Parse(handSide[1].Split(' ')[2]) / divisor;
        //     LRingValue = float.Parse(handSide[1].Split(' ')[3]) / divisor;
        //     LPinkyValue = float.Parse(handSide[1].Split('+')[0].Split(' ')[4]) / divisor;
        // }
    }

    void ControlHandByKeyboard()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RThumbValue -= 0.1f;
            RIndexValue -= 0.1f;
            RMiddleValue -= 0.1f;
            RRingValue -= 0.1f;
            RPinkyValue -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RThumbValue += 0.1f;
            RIndexValue += 0.1f;
            RMiddleValue += 0.1f;
            RRingValue += 0.1f;
            RPinkyValue += 0.1f;
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
        animator.SetFloat("R_Thumb", RThumbValue);
        animator.SetFloat("R_Index", RIndexValue);
        animator.SetFloat("R_Middle", RMiddleValue);
        animator.SetFloat("R_Ring", RRingValue);
        animator.SetFloat("R_Pinky", RPinkyValue);

        // Left
        animator.SetFloat("L_Thumb", LThumbValue);
        animator.SetFloat("L_Index", LIndexValue);
        animator.SetFloat("L_Middle", LMiddleValue);
        animator.SetFloat("L_Ring", LRingValue);
        animator.SetFloat("L_Pinky", LPinkyValue);
    }

    // Update is called once per frame
    void FixedUpdate()
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
