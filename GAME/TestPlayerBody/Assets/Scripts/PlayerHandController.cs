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

    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.FindObjectOfType<MqttProtocol>();
        animator = GetComponent<Animator>();
    }

    void AnimateHand()
    {
        animator.SetFloat("Index", RIndexValue);
        animator.SetFloat("Middle", RMiddleValue);
        animator.SetFloat("Thumb", RThumbValue);
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }
}
