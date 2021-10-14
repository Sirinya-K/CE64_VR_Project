using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    private MqttProtocol mqtt;
    private Animator animator;

    public float RIndexValue;
    public float RMiddleValue;
    public float RThumbValue;

    private int divisor = 20;
    private string mqttTempData = "";

    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.FindObjectOfType<MqttProtocol>();
        animator = GetComponent<Animator>();
    }

    void ControlHandByMqtt()
    {
        string[] handSide = mqtt.data.Split(':');
        if(handSide[0] == "hr")
        {
            RIndexValue = float.Parse(handSide[1].Split(' ')[1])/divisor;
            RMiddleValue = float.Parse(handSide[1].Split(' ')[2])/divisor;
            RThumbValue = float.Parse(handSide[1].Split(' ')[0])/divisor;
        }
    }

    void ControlHandByKeyboard()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            RIndexValue -= 0.1f;
            RMiddleValue -= 0.1f;
            RThumbValue -= 0.1f;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            RIndexValue += 0.1f;
            RMiddleValue += 0.1f;
            RThumbValue += 0.1f;
        }
    }

    void AnimateHand()
    {
        animator.SetFloat("Index", RIndexValue);
        animator.SetFloat("Middle", RMiddleValue);
        animator.SetFloat("Thumb", RThumbValue);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(mqttTempData != mqtt.data)
        {
            Debug.Log(mqtt.data);
            mqttTempData = mqtt.data;
            ControlHandByMqtt();
        }
        else if(Input.anyKeyDown)
        {
            ControlHandByKeyboard();
        }

        AnimateHand();
    }
}
