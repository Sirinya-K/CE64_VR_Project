using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public AudioSource footStepSound;

    public MqttProtocol mqtt;

    // public XRNode inputSource;
    public float speed = 1.8f;
    public float additionalHeight = 0.2f;

    // private Vector2 inputAxis;
    private CharacterController character;

    private XRRig rig;

    private Vector3 direction;

    private Quaternion headYaw;

    private float fallingSpeed;

    private Vector3 lastPosition;

    public bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();

        lastPosition = transform.position;
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

        // hl:0+6 6
        if ("hl" == mqtt.data.Split(':')[0])
        {
            string data = mqtt.data.Split(':')[1];
            string mode = data.Split('+')[0];
            if (mode == "0") // Move Character
            {
                moving = true;
                string moveData = data.Split('+')[1];
                ControlMovementByMqtt(moveData);
            }
        }
        if (Input.anyKey)
        {
            moving = true;
            ControlMovementByKeyboard();
        }

        // Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y);
        // character.Move(direction * Time.fixedDeltaTime * speed);

        fallingSpeed = -10;
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

        // [Foot Step Sound]
        if (lastPosition != transform.position && footStepSound.isPlaying == false && moving == true)
        {
            Debug.Log("Moving");
            footStepSound.Play();
        }
        else if (lastPosition == transform.position && (footStepSound.isPlaying == true || (footStepSound.isPlaying == false && moving == true)))
        {
            moving = false;
            Debug.Log("Not Moving");
            footStepSound.Stop();
        }
        lastPosition = transform.position;
    }
}