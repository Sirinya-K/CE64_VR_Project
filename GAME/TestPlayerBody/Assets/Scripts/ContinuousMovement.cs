using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public MqttProtocol mqtt;

    // public XRNode inputSource;
    public float speed = 1;
    public float additionalHeight = 0.2f;

    // private Vector2 inputAxis;
    private CharacterController character;

    private XRRig rig;

    private Vector3 direction;

    private string mqttCurrentData = "";

    private Quaternion headYaw;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }
    // Update is called once per frame
    // void Update()
    // {
    //     InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
    //     device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    // }

    // *** set ให้ตัว capsule ของ character เคลื่อนไหวตาม headset
    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    void ControlMovementByMqtt(string currentData)
    {
        // currentData = 5 5 --> idle
        // currentData = 1 1 --> F L
        // currentData = 9 9 --> B R
        sbyte fb = sbyte.Parse(currentData.Split(' ')[0]); // 8-bit integers
        sbyte lr = sbyte.Parse(currentData.Split(' ')[1]);

        if (lr > 6) // Left
        {
            direction = headYaw * new Vector3(-1, 0, 0);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
        if (lr < 4) // Right
        {
            direction = headYaw * new Vector3(1, 0, 0);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
        if (fb > 6) // Backward
        {
            direction = headYaw * new Vector3(0, 0, -1);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
        if (fb < 4) //Forward
        {
            direction = headYaw * new Vector3(0, 0, 1);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
    }

    void ControlMovementByKeyboard()
    {
        if (Input.GetKey(KeyCode.A)) // Left
        {
            direction = headYaw * new Vector3(-1, 0, 0);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D)) // Right
        {
            direction = headYaw * new Vector3(1, 0, 0);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S)) // Backward
        {
            direction = headYaw * new Vector3(0, 0, -1);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
        if (Input.GetKey(KeyCode.W)) //Forward
        {
            direction = headYaw * new Vector3(0, 0, 1);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        // *** set ให้ player เคลื่อนไหวตามทิศทางของ headset
        headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        // Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        if ("hl" == mqtt.data.Split(':')[0])
        {
            mqttCurrentData = mqtt.data.Split('+')[1];
            ControlMovementByMqtt(mqttCurrentData);
        }
        if (Input.anyKey)
        {
            ControlMovementByKeyboard();
        }

        // Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y);
        // character.Move(direction * Time.fixedDeltaTime * speed);
    }
}